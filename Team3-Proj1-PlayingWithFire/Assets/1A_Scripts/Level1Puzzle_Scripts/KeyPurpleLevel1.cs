using UnityEngine;

namespace _1A_Scripts.Level1Puzzle_Scripts
{
    /// <summary>
    /// This goes on the PARENT of the key -- this script here handled extra behaviour separate from other keys.
    /// </summary>
    public class KeyPurpleLevel1 : MonoBehaviour
    {
        public static KeyPurpleLevel1 Instance {get; private set;}
        [SerializeField] private GameObject poi;
        [SerializeField] private GameObject bubble;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        private bool hasCollected;
        void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            bubble.SetActive(true);
            poi.SetActive(false);
            hasCollected = false;
        }

        public void CollectPurple()
        {
            if (hasCollected) return;
            audioSource.PlayOneShot(audioClip);
            poi.SetActive(true);
            bubble.SetActive(false);
            hasCollected = true;
        }
    }
}
