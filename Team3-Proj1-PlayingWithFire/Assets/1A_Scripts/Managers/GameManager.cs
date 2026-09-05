using _1A_Scripts.Player;
using _1A_Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _1A_Scripts.Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public bool IsPaused { get; private set; }

        private const string MainMenuScene = "MainMenu";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            StartGame();
        }

        private void Update()
        {
            // Dev cheat: F12 skips to the next scene in the build order.
            if (Input.GetKeyDown(KeyCode.F12))
            {
                int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

                if (nextIndex < SceneManager.sceneCountInBuildSettings)
                {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene(nextIndex);
                }
                else
                {
                    Debug.LogWarning("F12: already on the last scene in the build.");
                }
            }
        }

        private void StartGame()
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

        public static void RespawnPlayer()
        {
            PlayerController.Instance.Respawn();
        }

        // Player survives scene loads (DontDestroyOnLoad) -- this is the one place that clears its
        // state (and any leftover HelpHints panel) so a new playthrough doesn't start still hurt/
        // keyed-up/paused from the last run.
        public void ResetForNewGame()
        {
            Time.timeScale = 1f;
            IsPaused = false;

            if (PlayerController.Instance)
            {
                PlayerController.Instance.ResetCheckpoint();
            }

            if (PlayerCombat.Instance)
            {
                PlayerCombat.Instance.ResetHealth();
            }

            if (HelpHints.Instance)
            {
                HelpHints.Instance.DisableAll();
            }
        }

        public void WinLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
