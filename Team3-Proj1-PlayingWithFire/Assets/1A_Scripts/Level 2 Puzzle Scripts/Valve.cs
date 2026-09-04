using _1A_Scripts.Level2Puzzles;
using UnityEngine;

namespace _1A_Scripts.Level_2_Puzzle_Scripts
{
    public class Valve : MonoBehaviour
    {
        [SerializeField] private GameObject promptDisplay;
        [SerializeField] private int maxTurns = 2;
        [SerializeField] private float targetAngle = 180;   // Renamed for clarity and changed to 180 for more visible effect.
        private float currentAngle = 0f;
        [SerializeField] private float rotationSpeed = 2f;     // More control over VFX
        [SerializeField] private AudioClip creakSound;
        [SerializeField] private GameObject valve;
        private AudioSource audioSource;    

        private int turnCount;
        private bool playerInRange;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            playerInRange = true;

            if (turnCount < maxTurns)
            {
                promptDisplay.gameObject.SetActive(true);
            }
            else if (turnCount >= maxTurns)    // Edge-case protection: If the player enters the trigger after the valve has been fully turned, the prompt will not display.
            {
                promptDisplay.gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            playerInRange = false;
            promptDisplay.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (playerInRange && turnCount < maxTurns && Input.GetKeyDown(KeyCode.E))
            {
                Turn();
            }
        } // just makes sure the conditions are met for player to turn

        private void Turn()
        {
            // Valve only because the pivot point is *not* set to the middle of the valve; this causes it to "jump".
            // I also swapped the rotation code out completely for smoother flow.

            // Smoothly moves the float towards the target angle.
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            // Applies said float directly to the Euler angles. :)
            valve.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
            
            turnCount++;

            if (turnCount >= maxTurns)
            {
                promptDisplay.gameObject.SetActive(false); 
            }

            LevelTwoPuzzleManager.Instance.CheckValves();
        } // sends info to puzzle manager once valve reaches the max turns and disables it

        public bool IsFullyTurned()
        {
            return turnCount >= maxTurns;
        }
    }
}
