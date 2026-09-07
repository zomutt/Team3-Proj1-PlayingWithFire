 using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    /// <summary>
    /// Formally known as: WaterProjectiles. Sorry, it just got way too confusing.
    /// Slightly modified to allow use of ObjectPooling because of performance concerns.
    /// Structure and code was intentionally left as intact as possible, just syntax and situational changes applied.
    /// This goes on each spawner.
    /// </summary>
    public class WaterOrbSpawner : MonoBehaviour // attach to enemy
    {
        [SerializeField] private float orbSpeed = 8f;
        [SerializeField] private float orbLifetime = 4f; // in case orb doesn't hit anything that destroys it, it goes away after 4 seconds

        // private void Update()
        // {
        //     // No longer needed, but this was copy pasted over to be used and called externally in ProjectileManager.cs
        //     // fireTimer += Time.deltaTime;
        //     //
        //     // if (fireTimer >= fireInterval)
        //     // {
        //     //     fireTimer = 0f;
        //     //     FireRing();
        //     // } 
        // }   // fires as soon as the interval is over

        public void FireRing()
        {
            // No longer needed if this is going on each spawner.
            //Vector3 origin = firePoint != null ? firePoint.position : transform.position;

            // for (int i = 0; i < orbCount; i++)
            // {
            //     float angle = gameObject.transform.rotation.eulerAngles.y;    // Gets the rotation of the spawner itself
            //     Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward; // adjusts ring to fit the amount of orbs 
            //
            //     GameObject orb = ProjectilePool.Instance.GetProjectile(gameObject.transform.position, gameObject.transform.rotation);
            //     Rigidbody rb = orb.GetComponent<Rigidbody>();
            //     rb.linearVelocity = direction * orbSpeed;
            // }
            
            float angle = gameObject.transform.rotation.eulerAngles.y;    // Gets the rotation of the spawner itself
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward; // adjusts ring to fit the amount of orbs,, now chooses which direction to shoot the orb in -- it's own angle.

            Debug.LogError($"[WaterOrbSpawner] FireRing reached. ProjectilePool.Instance null? {ProjectilePool.Instance == null}. direction={direction}"); // TEMP diagnostic

            GameObject orb = ProjectilePool.Instance.GetProjectile(gameObject.transform.position, gameObject.transform.rotation);

            Debug.LogError($"[WaterOrbSpawner] orb null? {orb == null}, active? {(orb ? orb.activeInHierarchy.ToString() : "N/A")}"); // TEMP diagnostic

            Rigidbody rb = orb.GetComponent<Rigidbody>();

            Debug.LogError($"[WaterOrbSpawner] rb null? {rb == null}"); // TEMP diagnostic

            rb.linearVelocity = direction * orbSpeed;
        }
    }
}
