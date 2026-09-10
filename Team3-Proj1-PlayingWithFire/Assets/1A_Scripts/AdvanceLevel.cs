using _1A_Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts
{
    public class AdvanceLevel : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                if (UIController.Instance)
                {
                    UIController.Instance.TransitionToScene(nextIndex);
                }
                else
                {
                    Debug.LogError("[AdvanceLevel] no UIController.Instance, loading scene with no transition");
                    SceneManager.LoadScene(nextIndex);
                }
            }
        }
    }
}
