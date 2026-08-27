using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _1A_Scripts.Managers
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        private const string CreditsScene = "Credits";        // Const string for the name of the credits scene, so we don't have to hardcode it in multiple places -- it basically lives forever
        private const string Level1Scene = "LevelOne";
        private const string MainMenuScene = "MainMenu";

        private static string previousScene;         // Also persists between scenes, so we can go back to the previous scene when we open the credits or help menu, except is shared between other objects

        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject confirmQuitPanel;
        [SerializeField] private GameObject settingsPanel;

        [SerializeField] private GameObject inv1;
        [SerializeField] private GameObject inv2;
        [SerializeField] private GameObject inv3;
        [SerializeField] private GameObject inv4;
        [SerializeField] private GameObject[] inv;

        [SerializeField] private Image inv1img;
        [SerializeField] private Image inv2img;
        [SerializeField] private Image inv3img;
        [SerializeField] private Image inv4img;
        

        [SerializeField] private GameObject invPanel;

        [SerializeField] private GameObject fadePanel;

        [SerializeField] private GameObject[] closeAllOnStart;   // Array to hold the key icons for easy management
        [SerializeField] private GameObject mmHelpPanel; // Help panel for the main menu

        private bool isMenuOpen = false; // Track whether the menu is open or closed
        private bool isQuitting = false; // Track whether the player is in the process of quitting

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void Start()
        {
            isQuitting = false; // Reset quitting state on start
            isMenuOpen = false; // Reset menu state on start.

            if (helpPanel)
            {
                helpPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("where's the help panel lol");
            }

            if (pauseMenu)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                Debug.LogWarning("no pause menu assigned, escape key's gonna do nothing visually");
            }

            if (confirmQuitPanel)
            {
                confirmQuitPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("no confirm quit panel assigned");
            }

            if (settingsPanel)
            {
                settingsPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("no settings panel assigned");
            }

            if (invPanel)
            {
                invPanel.SetActive(true);
            }

            if (closeAllOnStart != null)
            {
                foreach (GameObject obj in closeAllOnStart)
                {
                    if (obj)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogWarning("no key icons assigned");
            }

            if (inv != null)
            {
                foreach (GameObject obj in inv)
                {
                    if (obj) 
                        obj.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickTogglePause();
            }

            if (isMenuOpen && Input.GetKeyDown(KeyCode.S))
            {
                OnClickToggleSettings();
            }

            if (isMenuOpen && Input.GetKeyDown(KeyCode.H))
            {
                OnClickToggleHelp();
            }

            if (isMenuOpen && Input.GetKeyDown(KeyCode.M))
            {
                OnClickQuitGame();
            }

            if (isMenuOpen && Input.GetKeyDown(KeyCode.C))
            {
                OnClickMainMenu();
            }

            if (isMenuOpen && Input.GetKeyDown(KeyCode.R))
            {
                OnClickBackToPauseMenu();
            }
        }

        // public void UpdateKeys(string color)
        // {
        //     if (color == "Red" && keyCellRed != null)
        //     {
        //         keyCellRed.SetActive(true);
        //     }
        //     else if (color == "Purple" && keyRunPurple != null)
        //     {
        //         keyRunPurple.SetActive(true);
        //     }
        //     else if (color == "Blue" && keyCrushBlue != null)
        //     {
        //         keyCrushBlue.SetActive(true);
        //     }
        //     else if (color == "Green" && keyFountainGreen != null)
        //     {
        //         keyFountainGreen.SetActive(true);
        //     }
        // }
        public void OnClickTogglePause()
        {
            if (!GameManager.Instance)
            {
                Debug.LogWarning("no GameManager in this scene, can't pause");
                return;
            }

            if (GameManager.Instance.IsPaused)
            {
                GameManager.Instance.Play();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (pauseMenu)
                {
                    pauseMenu.SetActive(false);
                }
                else
                {
                    Debug.LogWarning("no pause menu assigned, escape key's gonna do nothing visually");
                }
                isMenuOpen = false;
            }
            else
            {
                GameManager.Instance.Pause();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (pauseMenu)
                {
                    pauseMenu.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("no pause menu assigned, escape key's gonna do nothing visually");
                }
                isMenuOpen = true;
            }
        }

        public void OnClickStartGame()      // ONLY for start menu.
        {
            previousScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(Level1Scene);
        }

        public void OnClickToggleHelp()
        {
            if (!helpPanel)
            {
                Debug.LogWarning("where's the help panel lol");
                return;
            }

            bool opening = !helpPanel.activeSelf;
            helpPanel.SetActive(opening);

            if (GameManager.Instance)
            {
                if (opening)
                {
                    GameManager.Instance.Pause();
                }
                else
                {
                    GameManager.Instance.Play();
                }
            }
            else
            {
                Debug.LogWarning("no GameManager in this scene, can't pause");
            }
        }

        public void OnClickToggleHelpSimple()
        {
            if (helpPanel == null)
            {
                Debug.LogWarning("where's the help panel lol");
                return;
            }

            helpPanel.SetActive(!helpPanel.activeSelf);
        }

        public void OnClickOpenCredits()
        {
            previousScene = SceneManager.GetActiveScene().name;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(CreditsScene);
        }

        public void OnClickReturnToPreviousScene()
        {
            if (previousScene != null)
            {
                SceneManager.LoadScene(previousScene);
            }
            else
            {
                Debug.LogWarning("nowhere to go back to, previousScene was never set");
            }
        }

        public void OnClickMainMenu()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(MainMenuScene);
        }

        public void OnClickQuitGame()      // Are you sure you want to quit?
        {
            if (confirmQuitPanel)
            {
                confirmQuitPanel.SetActive(true);
            }
            else
            {
                Debug.LogWarning("no confirm quit panel assigned");
            }

            if (pauseMenu)
            {
                pauseMenu.SetActive(false);
            }
        }

        public void OnClickCancelQuit()        // We don't want to quit after all, let's go back to the pause menu.
        {
            if (confirmQuitPanel != null)
            {
                confirmQuitPanel.SetActive(false);
            }

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
        }

        public void OnClickConfirmQuit()     // We are sure we want to quit :(
        {
            Application.Quit();
        }

        public void OnClickBackToPauseMenu()      // Shows the pause menu without touching Time.timeScale, for going back from Settings etc.
        {
            if (settingsPanel)
            {
                settingsPanel.SetActive(false);
            }

            if (pauseMenu)
            {
                pauseMenu.SetActive(true);
            }
            else
            {
                Debug.LogWarning("no pause menu assigned, escape key's gonna do nothing visually");
            }
        }

        public void OnClickToggleSettings()
        {
            if (settingsPanel)
            {
                settingsPanel.SetActive(!settingsPanel.activeSelf);
            }
            else
            {
                Debug.LogWarning("no settings panel assigned");
            }
        }

        public void OnClickMMHelp()    // Only for use on main menu
        {
            if (mmHelpPanel != null)
            {
                helpPanel.SetActive(!mmHelpPanel.activeSelf);
            }
            else
            {
                Debug.LogWarning("where's the help panel lol");
            }
        }
    }
}
