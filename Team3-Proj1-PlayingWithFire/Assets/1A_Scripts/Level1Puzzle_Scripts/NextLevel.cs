using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using _1A_Scripts.Managers;

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
            if (other.CompareTag("Player") && countdownRoutine == null)
            {
                countdownRoutine = StartCoroutine(CountdownRoutine());
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

                yield return new WaitForSecondsRealtime(1f);
                secondsLeft--;
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            yield return UIController.Instance.FadeOut();

            SceneManager.LoadScene(NextScene);
        }
    }
}
