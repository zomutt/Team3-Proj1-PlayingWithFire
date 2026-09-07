using _1A_Scripts.Level_3_scripts;
using _1A_Scripts.Player;
using UnityEngine;
using System.Collections;

namespace _1A_Scripts.Level1Puzzle_Scripts
{
    /// <summary>
    /// This should only go on level 3 orbs.
    /// </summary>
    public class WaterOrb : MonoBehaviour
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float orbLifetime = 4f;

        private void OnEnable()
        {
            StartCoroutine(OrbLifetime());
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Player")) return;
            
            PlayerCombat.Instance.TakeDamage(damage);
            ProjectilePool.Instance.ReleaseProjectile(gameObject);
        }
        private IEnumerator OrbLifetime()
        {
            yield return new WaitForSeconds(orbLifetime);
            ProjectilePool.Instance.ReleaseProjectile(gameObject);    
        }
    }
}
