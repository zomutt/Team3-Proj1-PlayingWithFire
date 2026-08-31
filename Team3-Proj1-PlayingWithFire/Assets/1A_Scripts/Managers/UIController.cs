using System.Collections;
using _1A_Scripts.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace _1A_Scripts.Managers
{
    public class UIController : MonoBehaviour
    {
        public static UIController Instance { get; private set; }

        private const string CreditsScene = "Credits";        // Const string for the name of the credits scene, so we don't have to hardcode it in multiple places -- it basically lives forever
        private const string Level1Scene = "Christie_BuildScene";
        private const string MainMenuScene = "MainMenu";

        private static string previousScene;         // Also persists between scenes, so we can go back to the previous scene when we open the credits or help menu, except is shared between other objects

        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject confirmQuitPanel;
        [SerializeField] private GameObject settingsPanel;

        [FormerlySerializedAs("keyCellRed")] [SerializeField] private GameObject keyRed;
        [FormerlySerializedAs("keyCrushBlue")] [SerializeField] private GameObject keyBlue;
        [FormerlySerializedAs("keyFountainGreen")] [SerializeField] private GameObject keyGreen;
        [FormerlySerializedAs("keyRunPurple")] [SerializeField] private GameObject keyPurple;

        [SerializeField] private GameObject invPanel;

        [SerializeField] private GameObject fadePanel;
        [SerializeField] private float fadeDuration = 0.5f;

        [SerializeField] private GameObject[] closeAllOnStart;   // Array to hold the key icons for easy management
        [SerializeField] private GameObject mmHelpPanel; // Help panel for the main menu

        private Image fadeImage;
        private bool isMenuOpen = false; // Track whether the menu is open or closed

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            if (fadePanel)
            {
                fadeImage = fadePanel.GetComponent<Image>();
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            isMenuOpen = false; // Reset menu state on start.

            if (fadePanel)
            {
                fadePanel.SetActive(false); // off by default so it's not blocking the screen during normal gameplay
            }

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
                foreach (var obj in closeAllOnStart)
                {
                    if (obj != null)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            else
            {
                Debug.LogWarning("no key icons assigned");
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

        public IEnumerator FadeOut()
        {
            fadePanel.SetActive(true);

            Color color = fadeImage.color;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = elapsed / fadeDuration;
                fadeImage.color = color;
                yield return null;
            }
            color.a = 1f;
            fadeImage.color = color;
        }

        public IEnumerator FadeIn()
        {
            Color color = fadeImage.color;
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                color.a = 1f - (elapsed / fadeDuration);
                fadeImage.color = color;
                yield return null;
            }
            color.a = 0f;
            fadeImage.color = color;

            fadePanel.SetActive(false);
        }

        public void UpdateKeys(string color)
        {
            if (color == "Red" && keyRed != null)
            {
                keyRed.SetActive(true);
            }
            else if (color == "Purple" && keyPurple != null)
            {
                keyPurple.SetActive(true);
            }
            else if (color == "Blue" && keyBlue != null)
            {
                keyBlue.SetActive(true);
            }
            else if (color == "Green" && keyGreen != null)
            {
                keyGreen.SetActive(true);
            }
        }
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
            // player survives scene loads (DontDestroyOnLoad), so gotta wipe the old checkpoint or a fresh run starts at CP2 like a liar
            if (PlayerController.Instance)
            {
                PlayerController.Instance.ResetCheckpoint();
            }

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
            if (!helpPanel)
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
            // same deal as start game -- catching it here too in case they rage quit to menu instead
            if (PlayerController.Instance)
            {
                PlayerController.Instance.ResetCheckpoint();
            }

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
            if (confirmQuitPanel)
            {
                confirmQuitPanel.SetActive(false);
            }

            if (pauseMenu)
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
            if (mmHelpPanel)
            {
                mmHelpPanel.SetActive(!mmHelpPanel.activeSelf);
            }
            else
            {
                Debug.LogWarning("where's the help panel lol");
            }

        }
    }
}
