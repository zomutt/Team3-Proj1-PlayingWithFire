using _1A_Scripts.Level1Puzzle_Scripts;
using _1A_Scripts.Level2Puzzles;
using _1A_Scripts.Managers;
using UnityEngine;

namespace _1A_Scripts
{
    public class Keys : MonoBehaviour
    {
        [SerializeField] private KeyColor keyColor;
        [SerializeField] private AudioClip audioClip;
        private AudioSource audioSource;

        [SerializeField] private GameObject POIRing;

        private IKeyCollector collector;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            POIRing.SetActive(true);

            // Whichever level's puzzle manager exists in this scene picks up the key -- no scene-name check needed.
            if (LevelOnePuzzleManager.Instance)
            {
                collector = LevelOnePuzzleManager.Instance;
            }
            else if (LevelTwoPuzzleManager.Instance)
            {
                collector = LevelTwoPuzzleManager.Instance;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            POIRing.SetActive(false);

            string color = keyColor.ToString().ToLower();
            collector?.CollectKey(color);

            if (UIController.Instance)
            {
                UIController.Instance.UpdateKeys(color);
            }

            if (audioClip)
            {
                audioSource.PlayOneShot(audioClip);
            }

            gameObject.SetActive(false);
        }
    }
}
