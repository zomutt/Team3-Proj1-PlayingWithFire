using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private const string CreditsScene = "Credits";
    private const string Level1Scene = "LevelOne";

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
        helpPanel.SetActive(false);
        pauseMenu.SetActive(false);
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
        if (GameManager.Instance.IsPaused)
        {
            GameManager.Instance.Play();
            pauseMenu.SetActive(false);
        }
        else
        {
            GameManager.Instance.Pause();
            pauseMenu.SetActive(true);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(Level1Scene);
    }

    public void OpenHelp()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        SceneManager.LoadScene(CreditsScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
