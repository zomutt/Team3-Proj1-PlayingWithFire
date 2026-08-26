using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private const string CreditsScene = "Credits";        // Const string for the name of the credits scene, so we don't have to hardcode it in multiple places -- it basically lives forever
    private const string Level1Scene = "LevelOne";
    private const string MainMenuScene = "Main Menu";

    private static string previousScene;         // Also persists between scenes, so we can go back to the previous scene when we open the credits or help menu, except is shared between other objects

    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject confirmQuitPanel;
    [SerializeField] private GameObject settingsPanel;

    [SerializeField] private GameObject keyCellRed;
    [SerializeField] private GameObject keyRunPurple;
    [SerializeField] private GameObject keyCrushBlue;
    [SerializeField] private GameObject keyFountainGreen;

    [SerializeField] private GameObject[] keyIcons;   // Array to hold the key icons for easy management

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
        isMenuOpen = false; // Reset menu state on start

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("where's the help panel lol");
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("no pause menu assigned, escape key's gonna do nothing visually");
        }

        if (confirmQuitPanel != null)
        {
            confirmQuitPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("no confirm quit panel assigned");
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("no settings panel assigned");
        }

        if (keyIcons != null)
        {
            foreach (GameObject keyIcon in keyIcons)
            {
                if (keyIcon != null)
                {
                    keyIcon.SetActive(false);
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
    }

    public void UpdateKeys(string color)
    {
        if (color == "Red" && keyCellRed != null)
        {
            keyCellRed.SetActive(true);
        }
        else if (color == "Purple" && keyRunPurple != null)
        {
            keyRunPurple.SetActive(true);
        }
        else if (color == "Blue" && keyCrushBlue != null)
        {
            keyCrushBlue.SetActive(true);
        }
        else if (color == "Green" && keyFountainGreen != null)
        {
            keyFountainGreen.SetActive(true);
        }
    }
    private void OnClickTogglePause()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("no GameManager in this scene, can't pause");
            return;
        }

        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.Play();

            if (pauseMenu != null)
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

            if (pauseMenu != null)
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

    public void OnClickOpenHelp()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            Debug.LogWarning("no GameManager in this scene, can't pause");
        }

        if (helpPanel != null)
        {
            helpPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("where's the help panel lol");
        }
    }

    public void OnClickCloseHelp()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Play();
        }
        else
        {
            Debug.LogWarning("no GameManager in this scene, can't unpause");
        }

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("where's the help panel lol");
        }
    }

    public void OnClickOpenCredits()
    {
        previousScene = SceneManager.GetActiveScene().name;
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
        SceneManager.LoadScene(MainMenuScene);
    }

    public void OnClickQuitGame()      // Are you sure you want to quit?
    {
        if (confirmQuitPanel != null)
        {
            confirmQuitPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("no confirm quit panel assigned");
        }

        if (pauseMenu != null)
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
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (pauseMenu != null)
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
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
        }
        else
        {
            Debug.LogWarning("no settings panel assigned");
        }
    }
}
