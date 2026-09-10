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
        private const string Level1Scene = "LevelOne";
        private const string Level2Scene = "LevelTwo";
        private const string Level3Scene = "LevelThree";
        private const string MainMenuScene = "MainMenu";
        private const string WinScene = "WIN";

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

        [Header("Brightness")]
        [SerializeField] private Image brightnessOverlay;
        [SerializeField] private Image darknessOverlay;

        [Header("Hit Panel")]
        [SerializeField] private Image hitPanel;
        [SerializeField] private float hitPanelMaxAlpha = 0.4f;
        [SerializeField] private float hitPanelFlashDuration = 0.15f;

        [Header("Bulk")]
        [SerializeField] private GameObject[] closeAllOnStart;   
        [SerializeField] private GameObject[] openAllOnStart;    
        [SerializeField] private GameObject mmHelpPanel; // Help panel for the main menu

        [Header("Health")] 
        [SerializeField] private Image healthBar;

        private Image fadeImage;
        private bool isMenuOpen = false;
        private float brightnessValue = 0f;

        [Header("Level 3")]
        public Image note;
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
        }
        
        private void DisableAll()
        {
            foreach (var obj in closeAllOnStart)
            {
                obj.SetActive(false);
            }
            note.gameObject.SetActive(false);
        }

        private void EnableAll()
        {
            foreach (var obj in openAllOnStart)
            {
                obj.SetActive(true);
            }

            healthBar.gameObject.SetActive(true);
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

            if (scene.name == WinScene)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

                if (fadePanel)
                {
                    StartCoroutine(FadeIn()); // smooth fade from black instead of an instant cut
                }

                return; // WIN just has two buttons -- none of the panel/HUD setup below applies to it
            }

            if (fadePanel)
            {
                fadePanel.SetActive(false); // off by default so it's not blocking the screen during normal gameplay
            }

            if (hitPanel)
            {
                Color color = hitPanel.color;
                color.a = 0f;
                hitPanel.color = color;
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
            // mouse is locked during gameplay, so this is the only way to get the pause menu open (and the mouse unlocked) without a button to click.
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnClickTogglePause();
            }

            if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                OnBrightnessChanged(brightnessValue + 0.1f);
            }

            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                OnBrightnessChanged(brightnessValue - 0.1f);
            }

            if (!pauseMenu.activeSelf)
            {
                return;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                settingsPanel.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.H))
            {
                helpPanel.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                confirmQuitPanel.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                settingsPanel.SetActive(false);
                helpPanel.SetActive(false);
                confirmQuitPanel.SetActive(false);
                pauseMenu.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                OnClickMainMenu();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                OnClickRestartLevel();
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

        // The one place every scene change should go through: fade to black, load, fade back in.
        // Yania's panel isn't wired into this yet -- simple fade only for now.
        public void TransitionToScene(string sceneName)
        {
            StartCoroutine(TransitionRoutine(sceneName));
        }

        public void TransitionToScene(int buildIndex)
        {
            string path = SceneUtility.GetScenePathByBuildIndex(buildIndex);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
            TransitionToScene(sceneName);
        }

        private IEnumerator TransitionRoutine(string sceneName)
        {
            yield return StartCoroutine(FadeOut());

            SceneManager.LoadScene(sceneName);

            yield return StartCoroutine(FadeIn());
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

        // UpdateKeys only ever turns these on -- this is the only way to clear them for a fresh playthrough.
        public void ResetKeys()
        {
            if (keyRed) keyRed.SetActive(false);
            if (keyBlue) keyBlue.SetActive(false);
            if (keyGreen) keyGreen.SetActive(false);
            if (keyPurple) keyPurple.SetActive(false);
        }

        // value range -1 (darkest) to 1 (brightest), 0 is normal.
        public void OnBrightnessChanged(float value)
        {
            if (!brightnessOverlay || !darknessOverlay)
            {
                Debug.LogWarning("brightness/darkness overlay not assigned");
                return;
            }

            if (value > 0.6f)
            {
                value = 0.6f;
            }
            else if (value < -0.6f)
            {
                value = -0.6f;
            }

            brightnessValue = value;

            if (value > 0)
            {
                Color brightColor = brightnessOverlay.color;
                brightColor.a = value;
                brightnessOverlay.color = brightColor;

                Color darkColor = darknessOverlay.color;
                darkColor.a = 0f;
                darknessOverlay.color = darkColor;
            }
            else
            {
                Color darkColor = darknessOverlay.color;
                darkColor.a = -value;
                darknessOverlay.color = darkColor;

                Color brightColor = brightnessOverlay.color;
                brightColor.a = 0f;
                brightnessOverlay.color = brightColor;
            }
        }

        public void FlashHitPanel()
        {
            if (!hitPanel)
            {
                Debug.LogWarning("no hit panel assigned");
                return;
            }

            StartCoroutine(HitPanelFlash());
        }

        private IEnumerator HitPanelFlash()
        {
            Color color = hitPanel.color;
            color.a = hitPanelMaxAlpha;
            hitPanel.color = color;

            yield return new WaitForSeconds(hitPanelFlashDuration);

            color.a = 0f;
            hitPanel.color = color;
        }

        public void UpdateHealthDisplay()
        {
            healthBar.fillAmount = PlayerCombat.Instance.PlayerHealth / PlayerCombat.Instance.PlayerMaxHealth;
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
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

            previousScene = SceneManager.GetActiveScene().name;
            TransitionToScene(Level1Scene);
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
            TransitionToScene(CreditsScene);
        }

        public void OnClickReturnToPreviousScene()
        {
            if (previousScene != null)
            {
                TransitionToScene(previousScene);
            }
            else
            {
                Debug.LogWarning("nowhere to go back to, previousScene was never set");
            }
        }

        public void OnClickMainMenu()
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            TransitionToScene(MainMenuScene);
        }

        public void OnClickRestartLevel()      // Start the current level over from scratch
        {
            if (GameManager.Instance)
            {
                GameManager.Instance.ResetForNewGame();
            }

            TransitionToScene(SceneManager.GetActiveScene().name);
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
