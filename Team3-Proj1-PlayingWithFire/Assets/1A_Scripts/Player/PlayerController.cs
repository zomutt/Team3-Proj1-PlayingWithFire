using System.Collections;
using _1A_Scripts.Managers;
using UnityEngine;

namespace _1A_Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private Transform respawnPoint;     // Only works for one respawn point, but we can add more later if we want to get fancy

        private int keysCollected;
        public int KeysCollected => keysCollected;

        private void Awake()
        {
            // Ensures there is only one instance of the player in a scene.
            if (Instance)
            {
                Debug.LogWarning("Multiple PlayerController instances found. Destroying duplicate.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            keysCollected = 0; // Initialize keys collected to 0 at the start of the game
        }

        public void AddKey()
        {
            keysCollected++;
        }

        public void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            PlayerMovement.Instance.ToggleMove();

            yield return UIController.Instance.FadeOut();

            // screen's fully black now, safe to teleport
            PlayerMovement.Instance.Teleport(respawnPoint.position);

            yield return UIController.Instance.FadeIn();

            PlayerMovement.Instance.ToggleMove();
        }
    }
}
