using UnityEngine;
using UnityEngine.Pool;
namespace _1A_Scripts.Level_3_scripts
{
    /// <summary>
    /// This lives on it's own GameObject and is a much more efficient way to handle projectiles than instantiating/destroying.
    /// By doing this, we have better performance and less lag, esp considering I'm having performance issues lol
    /// </summary>
    public class ProjectilePool : MonoBehaviour
    {
        public static ProjectilePool Instance;
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private int projectileCapacity;
        private ObjectPool<GameObject> projectilePool;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            
            projectilePool = new ObjectPool<GameObject>(
                createFunc: SpawnProjectile,
                actionOnGet: projectile => projectile.gameObject.SetActive(true),
                actionOnRelease: projectile => projectile.gameObject.SetActive(false),
                actionOnDestroy: projectile => Destroy(projectile.gameObject),
                collectionCheck: true,
                defaultCapacity: projectileCapacity,
                maxSize: 30 // High estimate while I gauge this.
            );
        }

        private GameObject SpawnProjectile()
        {
            return Instantiate(projectilePrefab);
        }

        // Called by the spawners.
        public GameObject GetProjectile(Vector3 position, Quaternion rotation)
        {
            GameObject projectile = projectilePool.Get();
            projectile.transform.SetPositionAndRotation(position, rotation);
            return projectile;
        }

        // Be free little fishy, back into the pool.
        public void ReleaseProjectile(GameObject projectile)
        {
            projectilePool.Release(projectile);
        }
    }
}
