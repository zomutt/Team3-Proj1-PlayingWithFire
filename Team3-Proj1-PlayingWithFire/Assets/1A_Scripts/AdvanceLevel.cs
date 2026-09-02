using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts
{
    public class AdvanceLevel : MonoBehaviour
    {
        private const string LevelOneScene = "Christie_BuildScene";
        private const string LevelTwoScene = "LevelTwo";
        private const string CreditsScene = "Credits";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == LevelOneScene)
            {
                SceneManager.LoadScene(LevelTwoScene);
            }
            else if (currentScene == LevelTwoScene)
            {
                SceneManager.LoadScene(CreditsScene);
            }
        }
    }
}
