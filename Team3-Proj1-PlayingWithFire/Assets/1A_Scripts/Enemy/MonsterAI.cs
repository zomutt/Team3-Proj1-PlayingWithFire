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
    [RequireComponent(typeof(AudioSource))]
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

        [Header("Death")]
        // Monster02_Die.anim (InPlace variant) is exactly 1s long. Kept as a field, not hardcoded,
        // in case the clip changes -- update this to match if it ever does.
        [SerializeField] private float deathAnimDuration = 1f;
        [SerializeField] private float deathFloorTime = 3f; // how long he lies there after the anim before Destroy

        [Header("Audio")] 
        private AudioSource audioSource;
        [SerializeField] private AudioClip[] hitClip;        // Arrays in case I decide to add in more SFX variety
        [SerializeField] private AudioClip[] attackClip;
        [SerializeField] private AudioClip[] aggroClip;
        [SerializeField] private AudioClip[] deathClip;
        
        

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
            audioSource = GetComponent<AudioSource>();
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
            TakeDamage(PlayerCombat.Instance.PlayerDamage); // already plays a hitClip itself, don't play another here
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

            PlayRandomClip(aggroClip);
        }

        // Guards against the prefab not having an AudioSource (throws otherwise) and an empty
        // clip array -- a thrown exception here used to be able to skip whatever ran after it,
        // e.g. Die()'s actual death trigger/Destroy call.
        private void PlayRandomClip(AudioClip[] clips)
        {
            if (!audioSource || clips == null || clips.Length == 0) return;
            audioSource.PlayOneShot(clips[Random.Range(0, clips.Length)]);
        }

        // Still slides some even with this -- probably needs an Animator-side fix, not a code one. (Yes. This was indeed the answer.)
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

            PlayRandomClip(attackClip);
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

            PlayRandomClip(hitClip);
        }

        private void Die()
        {
            isDead = true;
            agent.isStopped = true;

            // Death can land mid-chase soooo... We gotta make it chill.
            animator.speed = 1f;

            // Dying mid-chase/attack left IsWalking or IsAttacking stuck true, which was enough
            // for a bool-gated transition to pull him right back out of Die once it let go --
            // looked like he stood back up. Clearing them didn't fix it either, so something in
            // the Animator Controller itself leaves the Die state unconditionally -- that needs
            // an actual Animator-graph fix. Duct tape for now: freeze the Animator on a fixed
            // timer instead. Polling the Animator's own state to decide when to freeze was racing
            // the same bug (it can leave Die before the clip visually finishes), so this waits a
            // known duration instead of asking the broken state machine what it's doing.
            animator.SetBool(IsWalking, false);
            animator.SetBool(IsAttacking, false);
            animator.SetTrigger(Die1);
            StartCoroutine(FreezeAfterDeathAnim());

            // Full lifetime = the death anim playing, then deathFloorTime lying there before he's removed.
            Destroy(gameObject, deathAnimDuration + deathFloorTime);

            PlayRandomClip(deathClip); // after the trigger/Destroy -- a missing AudioSource shouldn't be able to skip those
        }

        private IEnumerator FreezeAfterDeathAnim()
        {
            yield return new WaitForSeconds(deathAnimDuration);
            animator.enabled = false; // locks him on whatever pose he's on so he can't transition out of Die
        }

        private IEnumerator Iframe()
        {
            canBeAttacked = false;
            yield return new WaitForSeconds(iframe);
            canBeAttacked = true;
        }
    }
}
