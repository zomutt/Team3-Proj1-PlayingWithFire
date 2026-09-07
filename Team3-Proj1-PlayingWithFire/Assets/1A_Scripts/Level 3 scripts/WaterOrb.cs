using System.Collections;
using _1A_Scripts.Player;
using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    /// <summary>
    /// This should only go on level 3 orbs.
    /// </summary>
    public class WaterOrb : FireReceiver
    {
        [SerializeField] private float damage = 10f;
        [SerializeField] private float orbLifetime = 4f;
        private bool hasReleased;   // edge case protection

        private void OnEnable()
        {
            hasReleased = false;
            StartCoroutine(OrbLifetime());
        }

        public override void ReceiveFire()
        {
            if (hasReleased) return;
            hasReleased = true;
            ProjectilePool.Instance.ReleaseProjectile(gameObject); // Orbs really don't feel like they should have life, so one shot, one kill.
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasReleased)  return;
            if (other.gameObject.layer == LayerMask.NameToLayer("CameraStatic"))   // Deletes the orb if it hits a wall
            {
                // Intentionally empty.
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                PlayerCombat.Instance.TakeDamage(damage);
            }
            else
            {
                return;
            }
            // ORB SFX WILL GO HERE
            hasReleased = true;     // Prevents double releasing -- this will literally mess everything up.
            ProjectilePool.Instance.ReleaseProjectile(gameObject);
        }
        private IEnumerator OrbLifetime()
        {
            yield return new WaitForSeconds(orbLifetime);
            if (hasReleased) yield break;
            hasReleased = true;
            ProjectilePool.Instance.ReleaseProjectile(gameObject);
        }
    }
}
