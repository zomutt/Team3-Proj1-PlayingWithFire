using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] string NextScene = "Credits";     // Hardcoded FOR NOW

    [SerializeField] private GameObject countdownPopup;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private float countdownTime = 3f;

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
        if (countdownPopup != null)
        {
            countdownPopup.SetActive(true);
        }

        float timeLeft = countdownTime;

        while (timeLeft > 0f)
        {
            if (countdownText != null)
            {
                countdownText.text = Mathf.CeilToInt(timeLeft).ToString();
            }

            yield return null;
            timeLeft -= Time.deltaTime;
        }

        SceneManager.LoadScene(NextScene);
    }//
}
