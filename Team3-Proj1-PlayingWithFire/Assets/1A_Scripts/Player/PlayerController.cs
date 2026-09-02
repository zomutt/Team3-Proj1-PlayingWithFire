using System.Collections;
using _1A_Scripts.Managers;
using EazyCamera;
using UnityEngine;

namespace _1A_Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController Instance { get; private set; }

        [SerializeField] private Transform respawnPoint1;
        [SerializeField] private Transform respawnPoint2;
        
        private AudioSource audioSource;
        [SerializeField] private AudioClip respawnClip;
        public bool hasHitCP2;

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
        }

        private void Start()
        {
            keysCollected = 0; // Initialize keys collected to 0 at the start of the game
            hasHitCP2 = false;
        }

        public void HitRespawn()
        {
            hasHitCP2 = true;
        }

        public void ResetCheckpoint()
        {
            hasHitCP2 = false;
        }
        public void AddKey()
        {
            keysCollected++;
        }
        public void Respawn()
        {
            audioSource.PlayOneShot(respawnClip);
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            PlayerMovement.Instance.ToggleMove();

            yield return UIController.Instance.FadeOut();

            // screen's fully black now, safe to teleport
            if (!hasHitCP2)
            {
                yield return PlayerMovement.Instance.Teleport(respawnPoint1.position);
            }
            else if (hasHitCP2)
            {
                yield return PlayerMovement.Instance.Teleport(respawnPoint2.position);
            }

            // otherwise the camera eases toward the new spot instead of just being there already
            if (EazyCam.Instance)
            {
                EazyCam.Instance.SnapToTarget();
            }

            yield return UIController.Instance.FadeIn();

            PlayerMovement.Instance.ToggleMove();
        }
    }
}
