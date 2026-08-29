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

        private void Start()
        {
            ResetGame();
        }

        public void ResetGame()
        {
            foreach (var key in keysArray)
            {
                key.SetActive(true);
            }

            greenKey.SetActive(false); // still gated behind the Crush puzzle -- only ActivateKey("green") should reveal this one

            HasRedKey = false;
            HasGreenKey = false;
            HasBlueKey = false;
            HasPurpleKey = false;
        }

        // Called by a puzzle once it's solved, to reveal that puzzle's key -- puzzles don't hold their own key references anymore.
        public void ActivateKey(string color)
        {
            switch (color)
            {
                case "red":
                    redKey.SetActive(true);
                    break;
                case "green":
                    greenKey.SetActive(true);
                    break;
                case "blue":
                    blueKey.SetActive(true);
                    break;
                case "purple":
                    purpleKey.SetActive(true);
                    break;
            }
        }

        public void CollectKeys(string color)
        {
            switch (color)
            {
                case "red":
                    HasRedKey = true;
                    redKey.SetActive(false);
                    redImg.gameObject.SetActive(true);
                    RedKeyMechanic();
                    break;
                case "green":
                    HasGreenKey = true;
                    greenKey.SetActive(false);
                    greenImg.gameObject.SetActive(true);
                    GreenKeyMechanic();
                    break;
                case "blue":
                    HasBlueKey = true;
                    blueKey.SetActive(false);
                    blueImg.gameObject.SetActive(true);
                    BlueKeyMechanic();
                    break;
                case "purple":
                    HasPurpleKey = true;
                    purpleKey.SetActive(false);
                    purpleImg.gameObject.SetActive(true);
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
            ww.StartCoroutine(ww.Fall());
        }

        private void GreenKeyMechanic()
        {
            StatueManager.Instance.UnlockStatues();
        }

        private void BlueKeyMechanic()
        {
            var ww = crushDoor.GetComponent<WaterWall>();
            ww.StartCoroutine(ww.Fall());
        }

        private void PurpleKeyMechanic()
        {
            var ww = runDoor.GetComponent<WaterWall>();
            ww.StartCoroutine(ww.Fall());
        }

        private bool hasAllThree()        // If the player has all three keys, they may unlock the last puzzle
        {
            return KeyCount >= 3;
        }
    }
}
