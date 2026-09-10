using System.Collections;
using _1A_Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Level_3_scripts
{
    public class EndGameTrigger : MonoBehaviour
    {
        [SerializeField] private string winSceneName = "WIN";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            StartCoroutine(EndGameSequence());
        }

        private IEnumerator EndGameSequence()
        {
            if (!UIController.Instance)
            {
                Debug.LogError("[EndGameTrigger] UIController.Instance is null, skipping fade and loading WIN scene directly");
                SceneManager.LoadScene(winSceneName);
                yield break;
            }

            yield return UIController.Instance.StartCoroutine(UIController.Instance.FadeOut());

            SceneManager.LoadScene(winSceneName);
        }
    }
}
