using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class Fireplace : FireReceiver
    {
        [SerializeField] private GameObject orbRed;
        [SerializeField] private GameObject fireVFX;
        [SerializeField] private GameObject poi;
        [SerializeField] private float burnTime = 2f;
        private float currentTime;
        private bool isBurning;


        private void Start()
        {
            orbRed.SetActive(false);
            fireVFX.SetActive(false);

            poi.SetActive(true);
            isBurning = false;
            currentTime = 0f;
        }

        public override void ReceiveFire()
        {
            if (isBurning) return;

            currentTime += Time.deltaTime;
            if (currentTime >= burnTime)
            {
                Ignite();
            }
        }

        private void Ignite()
        {
            isBurning = true;
            orbRed.SetActive(true);
            fireVFX.SetActive(true);
            poi.SetActive(false);
        }
    }
}