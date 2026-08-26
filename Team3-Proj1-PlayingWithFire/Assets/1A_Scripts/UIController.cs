using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    private const string CreditsScene = "Credits";
    private const string Level1Scene = "LevelOne";

    private static string previousScene;

    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject pauseMenu;

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
        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UIController: helpPanel is not assigned.");
        }

        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UIController: pauseMenu is not assigned.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("UIController: GameManager.Instance is null.");
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
                Debug.LogWarning("UIController: pauseMenu is not assigned.");
            }
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
                Debug.LogWarning("UIController: pauseMenu is not assigned.");
            }
        }
    }

    public void PlayGame()      // ONLY for start menu.
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(Level1Scene);
    }

    public void OpenHelp()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Pause();
        }
        else
        {
            Debug.LogWarning("UIController: GameManager.Instance is null.");
        }

        if (helpPanel != null)
        {
            helpPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("UIController: helpPanel is not assigned.");
        }
    }

    public void CloseHelp()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Play();
        }
        else
        {
            Debug.LogWarning("UIController: GameManager.Instance is null.");
        }

        if (helpPanel != null)
        {
            helpPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("UIController: helpPanel is not assigned.");
        }
    }

    public void OpenCredits()
    {
        previousScene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(CreditsScene);
    }

    public void ReturnToPreviousScene()
    {
        if (previousScene != null)
        {
            SceneManager.LoadScene(previousScene);
        }
        else
        {
            Debug.LogWarning("UIController: previousScene was never set.");
        }
    }

    public void QuitGame()      // Are you sure you want to quit?
    {
        Application.Quit();
    }

    public void OnClickConfirmQuit()     // We are sure we want to quit :(
    {
        Application.Quit();
    }
}
