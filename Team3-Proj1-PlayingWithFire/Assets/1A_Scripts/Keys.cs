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

        [SerializeField] private GameObject POIRing;

        private IKeyCollector collector;

        private void Start()
        {
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
                // Not audioSource.PlayOneShot -- deactivating this object right below would cut
                // the sound off instantly, since disabling a GameObject stops everything on it,
                // including audio already playing. PlayClipAtPoint spawns its own short-lived
                // object to play the clip, so it survives this one being deactivated.
                AudioSource.PlayClipAtPoint(audioClip, transform.position);
            }

            gameObject.SetActive(false);
        }
    }
}
