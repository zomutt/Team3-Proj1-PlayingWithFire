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

            if (UIController.Instance)
            {
                UIController.Instance.TransitionToScene(winSceneName);
            }
            else
            {
                Debug.LogError("[EndGameTrigger] no UIController.Instance, loading WIN scene with no transition");
                SceneManager.LoadScene(winSceneName);
            }
        }
    }
}
