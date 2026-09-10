using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class BossFlyToFire : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private float speed = 3f;
        private bool isFlying;

        public void StartFlying()
        {
            isFlying = true;
        }

        private void Update()
        {
            if (!isFlying || !firePoint) return;

            transform.position = Vector3.MoveTowards(transform.position, firePoint.position, speed * Time.deltaTime);
        }
    }
}
