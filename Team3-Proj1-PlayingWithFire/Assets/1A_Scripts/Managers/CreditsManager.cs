using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    public class CreditsManager : MonoBehaviour
    {
        private const string MainMenuScene = "MainMenu";

        private void Awake()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        public void OnClickMainMenu()
        {
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
