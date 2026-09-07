using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class ProjectileSpawner : MonoBehaviour
    {
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private float projectileSpeed = 8f;
        private float spawnTimer;

        private void Update()
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                spawnTimer = spawnInterval;
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            GameObject projectile = ProjectilePool.Instance.GetProjectile(transform.position, transform.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.linearVelocity = transform.forward * projectileSpeed;
        }
    }
}
