using _1A_Scripts.Enemy;
using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    // Called directly by LeverPuzzle the instant the puzzle's solved -- no dependency on the
    // vent's flight/arrival actually working, since that's cosmetic only now.
    public class BossDeathSequence : MonoBehaviour
    {
        [SerializeField] private Animator bossAnimator;
        [SerializeField] private BossFlyToFire bossFlyToFire;
        [SerializeField] private ProjectileManager projectileManager;

        public void TriggerDeath()
        {
            if (bossAnimator)
            {
                bossAnimator.SetBool("AirDuct", true);
                bossAnimator.enabled = false; // stop it from fighting the manual move to the fire
            }
            else
            {
                Debug.LogError("[BossDeathSequence] bossAnimator not assigned, AirDuct will never fire");
            }

            if (bossFlyToFire)
            {
                bossFlyToFire.StartFlying();
            }
            else
            {
                Debug.LogError("[BossDeathSequence] bossFlyToFire not assigned, boss won't move to the fire");
            }

            if (projectileManager)
            {
                projectileManager.StopFiring();
            }
            else
            {
                Debug.LogError("[BossDeathSequence] projectileManager not assigned, orbs will keep spawning after boss death");
            }

            DestroyAllMinions();
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
