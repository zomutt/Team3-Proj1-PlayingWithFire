using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class Fireplace : FireReceiver
    {
        [SerializeField] private GameObject orbRed;
        [SerializeField] private GameObject fireVFX;
        [SerializeField] private GameObject poi;


        private void Start()
        {
            orbRed.SetActive(false);
            fireVFX.SetActive(false);
            
            poi.SetActive(true);
        }

        public override void ReceiveFire()
        {
            poi.SetActive(false);
            orbRed.SetActive(true);
            fireVFX.SetActive(true);
        }
    }
}
