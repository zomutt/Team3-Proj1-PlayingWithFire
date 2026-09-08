using _1A_Scripts.Level2Puzzles;
using UnityEngine;

namespace _1A_Scripts.Level_2_Puzzle_Scripts
{
    public class Valve : MonoBehaviour
    {
        [SerializeField] private int maxTurns = 2;
        [SerializeField] private float targetAngle = 180;   // Renamed for clarity and changed to 180 for more visible effect.
        private float currentAngle = 0f;
        private float pendingTargetAngle = 0f;   // the notch currentAngle is currently easing towards
        [SerializeField] private float rotationSpeed = 2f;     // More control over VFX
        [SerializeField] private AudioClip creakSound;
        [SerializeField] private GameObject valve;
        [SerializeField] private GameObject poi;
        private AudioSource audioSource;

        private int turnCount;
        private bool playerInRange;

        private void Start()
        {
            if (poi) poi.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            playerInRange = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            playerInRange = false;
        }

        private void Update()
        {
            if (playerInRange && turnCount < maxTurns && Input.GetKeyDown(KeyCode.E))
            {
                Turn();
            }

            if (Mathf.Approximately(currentAngle, pendingTargetAngle)) return; // eases every frame instead of jumping on keypress
            currentAngle = Mathf.MoveTowards(currentAngle, pendingTargetAngle, rotationSpeed * Time.deltaTime);
            valve.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
        } // just makes sure the conditions are met for player to turn

        private void Turn()
        {
            // Valve only because the pivot point is *not* set to the middle of the valve; this causes it to "jump".
            turnCount++;
            pendingTargetAngle = targetAngle * turnCount / maxTurns;

            if (turnCount >= maxTurns)
            {
                if (poi) poi.SetActive(true);
            }

            LevelTwoPuzzleManager.Instance.CheckValves();
        } // sends info to puzzle manager once valve reaches the max turns and disables it

        public bool IsFullyTurned()
        {
            return turnCount >= maxTurns;
        }
    }
}
