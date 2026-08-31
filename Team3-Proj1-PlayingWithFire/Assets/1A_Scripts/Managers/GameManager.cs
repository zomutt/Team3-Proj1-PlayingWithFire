using _1A_Scripts.Player;
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
        }

        private void Start()
        {
            StartGame();
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

        public void WinLevel()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(MainMenuScene);
        }
    }
}
