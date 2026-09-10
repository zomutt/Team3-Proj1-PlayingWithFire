using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class BossCameraSwitch : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Camera bossCamera;
        [SerializeField] private float switchBackDelay = 5f;

        private bool isSwitched;
        private float switchTime;

        private void Awake()
        {
            if (!playerCamera)
            {
                playerCamera = Camera.main; // defaults to whatever's tagged MainCamera
            }

            // Force the correct starting state -- don't trust whatever's checked in the Hierarchy.
            if (playerCamera) playerCamera.gameObject.SetActive(true);

            bossCamera.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isSwitched) return;

            if (Time.time - switchTime >= switchBackDelay)
            {
                isSwitched = false;
                bossCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
            }
        }

        public void SwitchToBossCam()
        {
            Debug.LogError($"[BossCameraSwitch] SwitchToBossCam called. playerCamera={playerCamera}, bossCamera={bossCamera}"); // TEMP diagnostic

            playerCamera.gameObject.SetActive(false);
            bossCamera.gameObject.SetActive(true);
            isSwitched = true;
            switchTime = Time.time;

            Debug.LogError($"[BossCameraSwitch] after switch: playerCamera.active={playerCamera.gameObject.activeSelf}, bossCamera.active={bossCamera.gameObject.activeSelf}"); // TEMP diagnostic
        }
    }
}
