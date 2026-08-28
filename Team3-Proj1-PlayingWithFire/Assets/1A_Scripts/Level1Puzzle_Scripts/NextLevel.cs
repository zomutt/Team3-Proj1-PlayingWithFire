using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Level1Puzzle_Scripts
{
    public class NextLevel : MonoBehaviour
    {
        [SerializeField] private string NextScene = "Credits";     // Hardcoded FOR NOW

        [SerializeField] private GameObject countdownPopup;
        [SerializeField] private TMP_Text countdownText;
        [SerializeField] private float countdownTime = 5f;

        private Coroutine countdownRoutine;

        private void OnTriggerEnter(Collider other)        // When the player enters the trigger area, the countdown starts.
        {
            if (other.CompareTag("Player"))
            {
                countdownRoutine = StartCoroutine(CountdownRoutine());
            }
        }

        private void OnTriggerExit(Collider other)            // The player may abort the countdown by leaving the trigger area before the countdown finishes.
        {
            if (other.CompareTag("Player") && countdownRoutine != null)
            {
                StopCoroutine(countdownRoutine);
                countdownRoutine = null;

                if (countdownPopup != null)
                {
                    countdownPopup.SetActive(false);
                }
            }
        }

        private IEnumerator CountdownRoutine()        // After x amount of seconds, the player is sent to the next scene.
        {
            if (countdownPopup)
            {
                countdownPopup.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Countdown popup not assigned");
            }

            int secondsLeft = Mathf.CeilToInt(countdownTime);

            while (secondsLeft > 0)
            {
                if (countdownText)
                {
                    countdownText.text = secondsLeft.ToString();
                }

                yield return new WaitForSeconds(1f);
                secondsLeft--;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(NextScene);
        }
    }
}
