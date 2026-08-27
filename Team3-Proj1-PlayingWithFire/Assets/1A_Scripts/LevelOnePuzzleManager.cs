using _1A_Scripts.Level1Puzzle_Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace _1A_Scripts
{
    /// <summary>
    /// This is a master script for all of Level One that controls puzzle behaviour. Puzzles may still have their own supporting scripts, but this is a 
    /// centralised way to reduce clutter in our main scripts so that they can be reused throughout the game.
    /// </summary>
    public class LevelOnePuzzleManager : MonoBehaviour
    {
        public static LevelOnePuzzleManager Instance;
        public bool HasRedKey { get; private set; }

        public bool HasGreenKey { get; private set; }

        public bool HasBlueKey { get; private set; }

        public bool HasPurpleKey { get; private set; }

        [Header("Keys")] [SerializeField] private GameObject[] keysArray; // For bulk processes
        [SerializeField] private GameObject redKey; // For individual behaviour
        [SerializeField] private Image redImg;
        
        [SerializeField] private GameObject greenKey;
        [SerializeField] private Image greenImg;
        
        [SerializeField] private GameObject blueKey;
        [SerializeField] private Image blueImg;
        
        [SerializeField] private GameObject purpleKey;
        [SerializeField] private Image purpleImg;
        public int KeyCount { get; private set; }

        [Header("Doors")] 
        [SerializeField] private GameObject cellDoor;
        [SerializeField] private GameObject crushDoor;
        [SerializeField] private GameObject runDoor;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void ResetGame()
        {
            foreach (var key in keysArray)
            {
                key.SetActive(false);
            }

            HasRedKey = false;
            HasGreenKey = false;
            HasBlueKey = false;
            HasPurpleKey = false;
        }

        public void CollectKeys(string color)
        {
            switch (color)
            {
                case "red":
                    HasRedKey = true;
                    redKey.SetActive(false);
                    RedKeyMechanic();
                    break;
                case "green":
                    HasGreenKey = true;
                    greenKey.SetActive(false);
                    GreenKeyMechanic();
                    break;
                case "blue":
                    HasBlueKey = true;
                    blueKey.SetActive(false);
                    BlueKeyMechanic();
                    break;
                case "purple":
                    HasPurpleKey = true;
                    purpleKey.SetActive(false);
                    PurpleKeyMechanic();
                    break;
            }

            KeyCount++;
            if (hasAllThree())
            {
                purpleKey.SetActive(true);
            }
        }

        private void RedKeyMechanic()
        {
            var ww = cellDoor.GetComponent<WaterWall>();
            ww.Instance.StartCoroutine(ww.Fall());
        }

        private void GreenKeyMechanic()
        {
            HasGreenKey = true;
        }

        private void BlueKeyMechanic()
        {
            var ww = crushDoor.GetComponent<WaterWall>();
            ww.Instance.StartCoroutine(ww.Fall());
        }

        private void PurpleKeyMechanic()
        {
            var ww = runDoor.GetComponent<WaterWall>();
            ww.Instance.StartCoroutine(ww.Fall());
        }

        private bool hasAllThree()        // If the player has all three keys, they may unlock the last puzzle
        {
            KeyCount = 3;
            return true;
        }
    }
}
