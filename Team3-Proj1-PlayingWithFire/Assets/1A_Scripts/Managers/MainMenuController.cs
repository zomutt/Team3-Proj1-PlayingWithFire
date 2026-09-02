using _1A_Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    public class MainMenuController : MonoBehaviour
    {
        private const string Level1Scene = "Christie_BuildScene";
        private const string CreditsScene = "Credits";

        [SerializeField] private GameObject mmHelpPanel;

        private void Start()
        {
            if (mmHelpPanel)
            {
                mmHelpPanel.SetActive(false);
            }
        }

        public void OnClickStartGame()
        {
            if (PlayerController.Instance)
            {
                PlayerController.Instance.ResetCheckpoint();
            }

            SceneManager.LoadScene(Level1Scene);
        }

        public void OnClickOpenCredits()
        {
            SceneManager.LoadScene(CreditsScene);
        }

        public void OnClickMMHelp()
        {
            if (mmHelpPanel)
            {
                mmHelpPanel.SetActive(!mmHelpPanel.activeSelf);
            }
        }

        public void OnClickConfirmQuit()
        {
            Application.Quit();
        }
    }
}
