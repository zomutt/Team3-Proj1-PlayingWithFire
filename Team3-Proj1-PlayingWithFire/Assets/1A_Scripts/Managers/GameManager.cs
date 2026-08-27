using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsPaused { get; private set; }

    private const string MainMenuScene = "Main Menu";

    [SerializeField] private GameObject runWaterWall; // Drops once all 4 keys are collected.
    [SerializeField] private GameObject[] statuePOIs; // Turn on once the player has any 3 of the 4 keys.

    public bool hasRedKey;
    public bool hasGreenKey;
    public bool hasBlueKey;
    public bool hasPurpleKey;
    public bool hasAllKeys;

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
        PlayerController.Instance.Respawn();
    }

    public void WinLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuScene);
    }

    public void ObtainKey(int keyIndex)
    {
        switch (keyIndex)
        {
            case 0:
                hasRedKey = true;
                break;
            case 1:
                hasBlueKey = true;
                break;
            case 2:
                hasGreenKey = true;
                break;
            case 3:
                hasPurpleKey = true;
                break;
        }
        hasAllKeys = hasRedKey && hasGreenKey && hasBlueKey && hasPurpleKey;

        //if (hasAllKeys && runWaterWall != null)
        //{
        //    runWaterWall.SetActive(false);
        //}

        int keysHeld = 0;
        if (hasRedKey) keysHeld++;
        if (hasGreenKey) keysHeld++;
        if (hasBlueKey) keysHeld++;
        if (hasPurpleKey) keysHeld++;

        if (keysHeld >= 3)
        {
            foreach (GameObject poi in statuePOIs)
            {
                if (poi != null)
                {
                    poi.SetActive(true);
                }
            }
        }
    }
}
