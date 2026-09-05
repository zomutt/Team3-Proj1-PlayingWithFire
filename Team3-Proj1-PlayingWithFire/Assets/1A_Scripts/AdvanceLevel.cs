using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts
{
    public class AdvanceLevel : MonoBehaviour
    {
        private const string LevelOneScene = "LevelOne";
        private const string LevelTwoScene = "LevelTwo";
        private const string LevelThreeScene = "LevelThree";
        private const string CreditsScene = "Credits";

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
                return;

            var currentScene = SceneManager.GetActiveScene().name;

            switch (currentScene)
            {
                case LevelOneScene:
                    SceneManager.LoadScene(LevelTwoScene);
                    break;
                case LevelTwoScene:
                    SceneManager.LoadScene(LevelThreeScene);
                    break;
                case LevelThreeScene:
                    SceneManager.LoadScene(CreditsScene);
                    break;
            }
        }
    }
}
