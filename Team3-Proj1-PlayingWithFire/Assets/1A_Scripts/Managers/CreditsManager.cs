using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    public class CreditsManager : MonoBehaviour
    {
        private const string MainMenuScene = "MainMenu";

        public void OnClickMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
