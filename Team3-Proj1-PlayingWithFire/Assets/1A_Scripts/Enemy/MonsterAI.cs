using System.Collections;
using _1A_Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace _1A_Scripts.Enemy
{
    /// <summary>
    /// This script handles enemy AI movement, animations, attack, etc etc
    /// </summary>
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(NavMeshAgent))]
    internal class MonsterAI : FireReceiver
    {
        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        private static readonly int IsAttacking = Animator.StringToHash("IsAttacking");
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int GetHit = Animator.StringToHash("GetHit");
        private static readonly int Die1 = Animator.StringToHash("Die");

        [Header("References")] [SerializeField]
        private Transform player;
        private GameObject[] playerObjs;

        [Header("Detection")] [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackExitBuffer = 0.5f; // stops attack/chase flickering when distance sits right at attackRange
        private bool isInAttackRange;

        [Header("Movement")] [SerializeField] private float moveSpeed = 3f;

        [Header("Combat")] [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private int damage = 10;
        [SerializeField] private float iframe = .3f;
        private bool canBeAttacked;

        [Header("Health")] [SerializeField] private int maxHealth = 30;
        private int currentHealth;

        private Animator animator;
        private NavMeshAgent agent;
        private float lastAttackTime;
        private bool isDead;
        private float nextSpeedLogTime; // TEMP: throttles the speed-debug log so it doesn't flood the console

        private const float AnimatedWalkSpeed = 1.832f; // The walk clips speed in m/s. God hates us. :^))

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            currentHealth = maxHealth;
            canBeAttacked = true;

            agent.speed = moveSpeed;
            agent.stoppingDistance = attackRange;

            // TEMP diagnostic: find out exactly what Unity thinks is going on before we touch anything else.
            bool foundNearby = NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas);
            Debug.LogError($"[MonsterAI] pos={transform.position} isOnNavMesh={agent.isOnNavMesh} " +
                           $"nearestValidPointFound={foundNearby} nearestPoint={hit.position} distanceToNearest={Vector3.Distance(transform.position, hit.position):F3}");

            // The model's pivot can sit slightly off the baked mesh even when it looks flush with the
            // floor visually -- snap onto the nearest valid point so isOnNavMesh doesn't come back false.
            if (!agent.isOnNavMesh && foundNearby)
            {
                agent.Warp(hit.position);
            }

            // TEMP diagnostic: find every "Player"-tagged object in the scene, in case there's more than one
            // and FindGameObjectWithTag is quietly grabbing the wrong one.
            playerObjs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in playerObjs)
            {
                Debug.LogError($"[MonsterAI] found Player-tagged object: {obj.name} at {obj.transform.position}");
            }

            if (!player)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                if (playerObj) // Sets target
                {
                    player = playerObj.transform;
                }
            }

            Debug.LogError($"[MonsterAI] player assigned = {player != null}"); // TEMP diagnostic
        }

        private void Update()
        {
            if (isDead || !player)
            {
                return;
            }

            if (!agent.isOnNavMesh) // agent isn't on baked NavMesh data -- every agent call below would throw
            {
                return;
            }

            var distance =
                Vector3.Distance(transform.position,
                    player.position); // Literally just calculates the distance between the enemy and player to check for chase condition

            if (Time.time >= nextSpeedLogTime) // TEMP diagnostic, throttled -- fires every frame-ish regardless of state
            {
                nextSpeedLogTime = Time.time + 0.3f;
                Debug.Log($"[MonsterAI] LIVE distance={distance:F2} chaseRange={chaseRange} enemyPos={transform.position} playerPos={player.position} isInAttackRange={isInAttackRange}");
            }

            // Hysteresis: once he's committed to attacking, don't drop back out of it until the player is
            // meaningfully farther away than attackRange. Otherwise tiny distance jitter right at the
            // boundary flips Attack/Chase back and forth every frame. Uses the straight-line distance
            // (not agent.remainingDistance) since that stays 0 until SetDestination has ever been called.
            if (isInAttackRange)
            {
                if (distance > attackRange + attackExitBuffer)
                {
                    isInAttackRange = false;
                }
            }
            else if (distance <= attackRange)
            {
                isInAttackRange = true;
            }

            if (isInAttackRange)
            {
                AttackPlayer();
            }
            else if (distance <= chaseRange)
            {
                ChasePlayer();
            }
            else
            {
                Idle();
            }
        }
        public override void ReceiveFire()
        {
            TakeDamage(PlayerCombat.Instance.PlayerDamage);
        }
        private void ChasePlayer() // Triggered when player gets too close, basically aggro/hate
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);

            // NavMeshAgent accelerates/brakes instead of moving at a flat speed, so the walk clip's
            // playback rate has to track real velocity each frame or it'll slide during those ramps.
            animator.speed = agent.velocity.magnitude / AnimatedWalkSpeed;

            if (Time.time >= nextSpeedLogTime) // TEMP diagnostic, throttled
            {
                nextSpeedLogTime = Time.time + 0.3f;
                Debug.Log($"[MonsterAI] agent.speed={agent.speed:F2} agent.velocity.magnitude={agent.velocity.magnitude:F2} " +
                          $"animator.speed={animator.speed:F2} remainingDistance={agent.remainingDistance:F2} pathPending={agent.pathPending}");
            }

            animator.SetBool(IsWalking, true);
            animator.SetBool(IsAttacking, false);
        }

        private void AttackPlayer() // Only triggered when close enough to player (very close, melee range)
        {
            agent.isStopped = true;
            animator.speed = 1f; // combat/hit/death clips should always play at normal speed
            animator.SetBool(IsWalking, false);

            bool cooldownIsOver =
                Time.time - lastAttackTime >= attackCooldown; // Obv there's gotta be a cooldown between attacks
            if (!cooldownIsOver)
            {
                return;
            }

            lastAttackTime = Time.time;
            animator.SetBool(IsAttacking, true);
            animator.SetTrigger(Attack);

            PlayerCombat.Instance.TakeDamage(damage);
        }

        private void Idle()
        {
            agent.isStopped = true;
            animator.speed = 1f;
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsAttacking, false);
        }

        private void TakeDamage(int amount)
        {
            if (isDead) return;

            if (!canBeAttacked) return;

            currentHealth -= amount;
            animator.SetTrigger(GetHit);

            if (currentHealth <= 0)
            {
                Die();
            }
            else
            {
                StartCoroutine(Iframe());
            }
        }

        private void Die()
        {
            isDead = true;
            agent.isStopped = true;
            animator.SetTrigger(Die1);
            Destroy(gameObject, 3f); // Allows the animation to actually play. Might get tweaked.
        }

        private IEnumerator Iframe()
        {
            canBeAttacked = false;
            yield return new WaitForSeconds(iframe);
            canBeAttacked = true;
        }
    }
}
