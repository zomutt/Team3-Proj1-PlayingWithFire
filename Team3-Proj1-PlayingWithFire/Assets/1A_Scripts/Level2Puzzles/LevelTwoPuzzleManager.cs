using _1A_Scripts.Level1Puzzle_Scripts;
using UnityEngine;

namespace _1A_Scripts.Level2Puzzles
{
    public class LevelTwoPuzzleManager : MonoBehaviour
    {
        public static LevelTwoPuzzleManager Instance;

        [Header("Misc")]
        [SerializeField] private GameObject[] ActivateOnStart;
        [SerializeField] private GameObject[] DeactivateOnStart;

        [Header("Keys")]
        [SerializeField] private GameObject keyRed;
        [SerializeField] private GameObject keyGreen;
        [SerializeField] private GameObject keyBlue;
        [SerializeField] private GameObject keyPurple;

        [Header("Waterfalls")]
        [SerializeField] private WaterWall redKeyWaterfall;
        [SerializeField] private WaterWall greenKeyWaterfall;   
        [SerializeField] private WaterWall brazierWaterfall;
        [SerializeField] private WaterWall valveWaterfall;
        [SerializeField] private WaterWall exitWaterfall; // inspector fields for waterfalls    

        [Header("Brazier Puzzle")]
        [SerializeField] private BrazierFire[] braziers;

        [Header("Valves")]
        [SerializeField] private Valve[] valves; // arrays for puzzles 

        private bool hasRedKey;
        private bool hasGreenKey;
        private bool hasPurpleKey;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            ActivateAll();
            DeactivateAll();
        }

        private void ActivateAll()
        {
            foreach (var go in ActivateOnStart)
            {
                go.SetActive(true);
            }
        }

        private void DeactivateAll()
        {
            foreach (var go in DeactivateOnStart)
            {
                go.SetActive(false);
            }
        }

        public void Activate(string keyColor)
        {
            switch (keyColor)
            {
                case "Red":
                    keyRed.SetActive(true);
                    break;
                case "Green":
                case "Blue":
                case "Purple":
                    break;
                default:
                    Debug.LogWarning($"KeyColor {keyColor} is missing or invalid. Proper format: Red");
                    break;
            }
        }

        public void CollectKey(string keyColor)
        {
            switch (keyColor)
            {
                case "red":
                    hasRedKey = true;
                    redKeyWaterfall.StartCoroutine(redKeyWaterfall.Fall());
                    break;
                case "green":
                    hasGreenKey = true;
                    greenKeyWaterfall.StartCoroutine(greenKeyWaterfall.Fall());
                    break;
                case "purple":
                    hasPurpleKey = true;
                    break;
                case "blue":
                    break;
                default:
                    Debug.LogWarning($"KeyColor {keyColor} is missing or invalid. Proper format: red");
                    break;
            } //drops waterfalls dedicated to each key

            if (hasRedKey && hasGreenKey && hasPurpleKey)
            {
                exitWaterfall.StartCoroutine(exitWaterfall.Fall()); // once player gets all 3 keys, it disables waterfall
            }
        }

        public void CheckBraziers()
        {
            foreach (BrazierFire brazier in braziers)
            {
                if (!brazier.IsCorrect())
                {
                    return;
                }
            }

            brazierWaterfall.StartCoroutine(brazierWaterfall.Fall()); // drops waterfall once it checks all braziers are correct
        }

        public void CheckValves()
        {
            foreach (Valve valve in valves)
            {
                if (!valve.IsFullyTurned())
                {
                    return;
                }
            }

            valveWaterfall.StartCoroutine(valveWaterfall.Fall()); // same logic here except for valves
        }
    }
}
