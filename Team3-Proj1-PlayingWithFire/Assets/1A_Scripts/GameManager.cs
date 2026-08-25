using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
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
}
