using System.Collections;
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
        private const string Level1Scene = "LevelOne";
        private const string Level2Scene = "LevelTwo";
        private const string Level3Scene = "LevelThree";
        private const string MainMenuScene = "MainMenu";

        private static string previousScene;         // Also persists between scenes, so we can go back to the previous scene when we open the credits or help menu, except is shared between other objects

        [Header("Panels")]
        [SerializeField] private GameObject helpPanel;
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private GameObject confirmQuitPanel;
        [SerializeField] private GameObject settingsPanel;

        [SerializeField] private GameObject HintCanvas;
        
        [Header("Keys")]
        [FormerlySerializedAs("keyCellRed")] [SerializeField] private GameObject keyRed;
        [FormerlySerializedAs("keyCrushBlue")] [SerializeField] private GameObject keyBlue;
        [FormerlySerializedAs("keyFountainGreen")] [SerializeField] private GameObject keyGreen;
        [FormerlySerializedAs("keyRunPurple")] [SerializeField] private GameObject keyPurple;

        [Header("Fade Panels")]
        [SerializeField] private GameObject fadePanel;
        [SerializeField] private float fadeDuration = 0.5f;

        [Header("Bulk")]
        [SerializeField] private GameObject[] closeAllOnStart;   
        [SerializeField] private GameObject[] openAllOnStart;    
        [SerializeField] private GameObject mmHelpPanel; // Help panel for the main menu

        [Header("Health")] 
        [SerializeField] private Image[] hearts;
        private SpriteRenderer sr;
        private SpriteRenderer originalSr;

        private Image fadeImage;
        private bool isMenuOpen = false;

        private void Awake()
        {
            if (Instance)
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

            SceneManager.sceneLoaded += OnSceneLoaded;

            FindHintCanvas(); // HintCanvas does not persist through scenes (caused issues), so we have to find it each time we start a level.
            
            sr = hearts[0].GetComponent<SpriteRenderer>();   // We only need to store one because they are all the same
        }
        
        private void DisableAll()
        {
            foreach (var obj in closeAllOnStart)
            {
                obj.SetActive(false);
            }
        }

        private void EnableAll()
        {
            foreach (var obj in openAllOnStart)
            {
                obj.SetActive(true);
            }

            foreach (var heart in hearts)
            {
                heart.gameObject.SetActive(true);
            }
        }

        private void FindHintCanvas()
        {
            HelpHints hintScript = FindFirstObjectByType<HelpHints>();
            if (hintScript)
            {
                HintCanvas = hintScript.gameObject;
                HintCanvas.SetActive(true);
            }
            else
            {
                Debug.LogWarning("no HelpHints found in this scene");
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
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

            DisableAll();
            EnableAll();
            FindHintCanvas();
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
            switch (color)
            {
                case "red" when keyRed:
                    keyRed.SetActive(true);
                    break;
                case "purple" when keyPurple:
                    keyPurple.SetActive(true);
                    break;
                case "blue" when keyBlue:
                    keyBlue.SetActive(true);
                    break;
                case "green" when keyGreen:
                    keyGreen.SetActive(true);
                    break;
            }
        }

        public void UpdateHealthDisplay(int currentHealth)
        {
            Color lostHeartColor = Color.black;
            lostHeartColor.a = 0.5f;   // 50% transparency

            // Redraws every heart every time instead of only the one that changed -- way less annoying than trying to track what was already lit up.
            for (int i = 0; i < hearts.Length; i++)
            {
                if (i < currentHealth)
                {
                    hearts[i].color = Color.white; // still got this heart
                }
                else
                {
                    hearts[i].color = lostHeartColor; // rip :(
                }
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
            // player, UI state, etc. all survive scene loads -- gotta wipe them or a fresh run starts
            // still hurt/keyed-up/at CP2 like a liar
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
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

            var opening = !helpPanel.activeSelf;
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
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(MainMenuScene);
        }

        public void OnClickRestartLevel()      // Start the current level over from scratch
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        }
    }
}
