using _1A_Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    public class MainMenuController : MonoBehaviour
    {
        private const string Level1Scene = "LevelOne";
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
            // Time.timeScale/IsPaused/HUD key icons all survive scene loads via GameManager/UIController --
            // gotta wipe them here or a fresh run can start still paused from wherever the last playthrough left off.
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

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
