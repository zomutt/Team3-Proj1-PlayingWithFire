using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class LeverPuzzle : MonoBehaviour
    {
        [SerializeField] private Lever lever1;
        [SerializeField] private Lever lever2;
        [SerializeField] private Lever lever3;
        [SerializeField] private Lever lever4;

        [SerializeField] private GameObject objectToActivate;
        [SerializeField] private BossCameraSwitch cameraSwitch;

        // Every FlyToBoss checks this itself -- no array of vents to wire up, nothing to forget.
        public static bool LeversSolved { get; private set; }

        private void Awake()
        {
            LeversSolved = false; // reset on scene load/restart, static fields don't reset on their own
        }

        public void CheckLevers()
        {
            if (lever1.IsUp() || !lever2.IsUp() || !lever3.IsUp() || lever4.IsUp()) return;
            objectToActivate.SetActive(true);
            LeversSolved = true;

            if (cameraSwitch)
            {
                cameraSwitch.SwitchToBossCam();
            }
            else
            {
                Debug.LogError("[LeverPuzzle] cameraSwitch not assigned, camera will never switch");
            }
        }
    }
}