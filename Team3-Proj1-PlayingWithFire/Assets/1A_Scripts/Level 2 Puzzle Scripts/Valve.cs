using _1A_Scripts.Level2Puzzles;
using TMPro;
using UnityEngine;

namespace _1A_Scripts.Level_2_Puzzle_Scripts
{
    public class Valve : MonoBehaviour
    {
        [SerializeField] private GameObject promptDisplay;
        [SerializeField] private int maxTurns = 2;
        [SerializeField] private float rotationStep = 45f;
        [SerializeField] private AudioClip creakSound;
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
        } // detects player and shows text

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
            transform.Rotate(0f, 0f, rotationStep); 
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
