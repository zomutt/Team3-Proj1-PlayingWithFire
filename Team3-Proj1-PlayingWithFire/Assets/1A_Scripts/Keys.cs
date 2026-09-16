using _1A_Scripts.Level1Puzzle_Scripts;
using _1A_Scripts.Level2Puzzles;
using _1A_Scripts.Level_3_scripts;
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
            else if (LevelThreePuzzleManager.Instance)
            {
                collector = LevelThreePuzzleManager.Instance;
            }

            // TEMP DIAGNOSTIC -- remove once we know why keys stop working after a full playthrough.
            Debug.Log($"[Keys diag] {gameObject.name} in scene '{gameObject.scene.name}': " +
                      $"collector={(collector == null ? "NULL" : collector.GetType().Name)}, " +
                      $"L1.Instance={(LevelOnePuzzleManager.Instance ? LevelOnePuzzleManager.Instance.GetInstanceID().ToString() : "null")}, " +
                      $"L2.Instance={(LevelTwoPuzzleManager.Instance ? LevelTwoPuzzleManager.Instance.GetInstanceID().ToString() : "null")}, " +
                      $"L3.Instance={(LevelThreePuzzleManager.Instance ? LevelThreePuzzleManager.Instance.GetInstanceID().ToString() : "null")}");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            POIRing.SetActive(false);

            string color = keyColor.ToString().ToLower();

            // TEMP DIAGNOSTIC -- remove once we know why keys stop working after a full playthrough.
            Debug.Log($"[Keys diag] OnTriggerEnter {color}: collector={(collector == null ? "NULL" : collector.GetType().Name)}, " +
                      $"UIController.Instance={(UIController.Instance ? "valid" : "NULL")}");

            collector?.CollectKey(color);

            if (UIController.Instance)
            {
                UIController.Instance.UpdateKeys(color);
            }

            if (audioClip)
            {
                AudioSource.PlayClipAtPoint(audioClip, transform.position); // SetActive(false) below would cut PlayOneShot off mid-sound
            }

            gameObject.SetActive(false);
        }
    }
}
