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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClickTogglePause();
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

    public void OnClickQuitGame()      // Are you sure you want to quit?
    {
        Application.Quit();
    }

    public void OnClickConfirmQuit()     // We are sure we want to quit :(
    {
        Application.Quit();
    }
}
