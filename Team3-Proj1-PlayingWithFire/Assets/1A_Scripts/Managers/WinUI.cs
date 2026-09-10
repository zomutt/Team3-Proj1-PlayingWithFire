using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    // Self-contained on purpose -- doesn't touch UIController.Instance at all,
    // so it still works even if that persistent object isn't around on this scene path.
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string creditsScene = "Credits";

        public void OnClickMainMenu()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(mainMenuScene);
        }

        public void OnClickCredits()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(creditsScene);
        }
    }
}
