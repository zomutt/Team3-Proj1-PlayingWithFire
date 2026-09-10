using _1A_Scripts.Enemy;
using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class FlyToBoss : MonoBehaviour
    {
        [SerializeField] private Transform boss;
        [SerializeField] private Animator bossAnimator;
        [SerializeField] private BossFlyToFire bossFlyToFire;
        [SerializeField] private ProjectileManager projectileManager;
        [SerializeField] private float speed = 5f;
        [SerializeField] private float arrivalDistance = 0.2f;

        private void Awake()
        {
            if (!boss)
            {
                Debug.LogError($"[FlyToBoss] {gameObject.name}: 'boss' not assigned, it will never move");
            }

            if (!bossAnimator)
            {
                Debug.LogError($"[FlyToBoss] {gameObject.name}: 'bossAnimator' not assigned, AirDuct will never fire");
            }

            if (!bossFlyToFire)
            {
                Debug.LogError($"[FlyToBoss] {gameObject.name}: 'bossFlyToFire' not assigned, boss won't fly to the fire");
            }
        }

        private void Update()
        {
            if (!LeverPuzzle.LeversSolved) return;
            if (!boss) return;

            transform.position = Vector3.MoveTowards(transform.position, boss.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, boss.position) <= arrivalDistance)
            {
                if (bossAnimator)
                {
                    bossAnimator.SetBool("AirDuct", true);
                }

                if (bossFlyToFire)
                {
                    bossFlyToFire.StartFlying();
                }

                if (projectileManager)
                {
                    projectileManager.StopFiring();
                }
                else
                {
                    Debug.LogError("[FlyToBoss] projectileManager not assigned, orbs will keep spawning after boss death");
                }

                DestroyAllMinions();
            }
        }

        private void DestroyAllMinions()
        {
            MonsterAI[] minions = FindObjectsByType<MonsterAI>(FindObjectsSortMode.None);
            foreach (var minion in minions)
            {
                Destroy(minion.gameObject);
            }
        }
    }
}
