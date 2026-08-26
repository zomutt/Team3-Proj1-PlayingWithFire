using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private const string MainMenuScene = "Main Menu";

    private Transform checkpoint; // Where the player respawns after touching water.

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        checkpoint = GameObject.FindGameObjectWithTag("Respawn").transform;
    }

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }

    public void Play()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void RespawnPlayer()
    {
        if (ScreenFader.Instance != null)
        {
            ScreenFader.Instance.FadeToBlackAndBack(() =>
            {
                PlayerController.Instance.transform.position = checkpoint.position;
            });
        }
        else
        {
            PlayerController.Instance.transform.position = checkpoint.position;
        }
    }

    public void WinLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }
}
