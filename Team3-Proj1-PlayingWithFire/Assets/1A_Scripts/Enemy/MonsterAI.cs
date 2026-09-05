using System.Collections;
using _1A_Scripts.Player;
using UnityEngine;
using UnityEngine.AI;

namespace _1A_Scripts.Enemy
{
    /// <summary>
    /// This script handles enemy AI movement, animations, attack, etc etc
    /// He slidin' still :( Gl;hf.
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

        [Header("Detection")] 
        [SerializeField] private float chaseRange = 10f;
        [SerializeField] private float attackRange = 2f;
        [SerializeField] private float attackExitBuffer = 0.5f; // Attempt to stop attack/chase flicker right at attackRange
        private bool isInAttackRange;

        [Header("Movement")] 
        [SerializeField] private float moveSpeed = 3f;

        [Header("Combat")] 
        [SerializeField] private float attackCooldown = 1.5f;
        [SerializeField] private int damage = 10;
        [SerializeField] private float iframe = .3f;
        private bool canBeAttacked;

        [Header("Health")] 
        [SerializeField] private int maxHealth = 30;
        private int currentHealth;

        private Animator animator;
        private NavMeshAgent agent;
        private float lastAttackTime;
        private bool isDead;

        private const float AnimatedWalkSpeed = 1.832f; // The walk clips speed in m/s. God hates us. :^))

        // If he needs to turn more than this many degrees, just snap to face the new direction instead of slowly turning -- a big turn take too long and looks like sliding.
        private const float SnapTurnAngle = 100f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            agent = GetComponent<NavMeshAgent>();
            currentHealth = maxHealth;
            canBeAttacked = true;

            agent.speed = moveSpeed;
            agent.stoppingDistance = attackRange;

            // Taking rotation over from the agent -- known Unity quirk
            agent.updateRotation = false;

            // Snap onto the mesh if he spawns just barely off it. Trying to rule out positioning issues.
            if (!agent.isOnNavMesh && NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                agent.Warp(hit.position);
            }

            if (player) return;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj) // Sets target
            {
                player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (isDead || !player)
            {
                return;
            }

            if (!agent.isOnNavMesh) return;

            var distance =
                Vector3.Distance(transform.position,
                    player.position); // Literally just calculates the distance between the enemy and player to check for chase condition

            // So he doesn't flicker between attack/chase
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
            RotateTowardsMovement();

            // Keeps the walk clip's pace matched to how fast he's actually moving
            animator.speed = agent.velocity.magnitude / AnimatedWalkSpeed;

            animator.SetBool(IsWalking, true);
            animator.SetBool(IsAttacking, false);
        }

        // Still slides some even with this -- probably needs an Animator-side fix, not a code one.
        private void RotateTowardsMovement()
        {
            Vector3 direction = agent.desiredVelocity;
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            if (Quaternion.Angle(transform.rotation, targetRotation) > SnapTurnAngle)
            {
                transform.rotation = targetRotation;
            }
            else
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                    agent.angularSpeed * Time.deltaTime);
            }
        }

        private void AttackPlayer() // Only triggered when close enough to player (very close, melee range)
        {
            agent.isStopped = true;
            animator.speed = 1f;
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

            // Death can land mid-chase soooo... We gotta make it chill.
            animator.speed = 1f;
            animator.SetTrigger(Die1);
            Destroy(gameObject, 3f);   // 3f allows the animation to actually play. Might get tweaked.
        }

        private IEnumerator Iframe()
        {
            canBeAttacked = false;
            yield return new WaitForSeconds(iframe);
            canBeAttacked = true;
        }
    }
}
