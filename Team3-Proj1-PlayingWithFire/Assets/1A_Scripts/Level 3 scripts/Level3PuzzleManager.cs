using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class LevelThreePuzzleManager : MonoBehaviour, IKeyCollector
    {
        public static LevelThreePuzzleManager Instance;

        public bool HasRedOrb { get; private set; }
        public bool HasGreenOrb { get; private set; }
        public bool HasBlueOrb { get; private set; }
        public bool HasPurpleOrb { get; private set; }

        [Header("Orbs")]
        [SerializeField] private GameObject redOrb;
        [SerializeField] private GameObject greenOrb;
        [SerializeField] private GameObject blueOrb;
        [SerializeField] private GameObject purpleOrb;

        [Header("Doors")]
        [SerializeField] private WaterWall redOrbDoor;
        [SerializeField] private WaterWall greenOrbDoor;
        [SerializeField] private WaterWall blueOrbDoor;
        [SerializeField] private WaterWall purpleOrbDoor;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        // Called by a puzzle once it's solved, to reveal that puzzle's orb.
        public void ActivateOrb(string color)
        {
            switch (color)
            {
                case "red":
                    if (redOrb) redOrb.SetActive(true);
                    break;
                case "green":
                    if (greenOrb) greenOrb.SetActive(true);
                    break;
                case "blue":
                    if (blueOrb) blueOrb.SetActive(true);
                    break;
                case "purple":
                    if (purpleOrb) purpleOrb.SetActive(true);
                    break;
            }
        }

        public void CollectKey(string color)
        {
            switch (color)
            {
                case "red":
                    HasRedOrb = true;
                    if (redOrbDoor) redOrbDoor.StartCoroutine(redOrbDoor.Fall());
                    break;
                case "green":
                    HasGreenOrb = true;
                    if (greenOrbDoor) greenOrbDoor.StartCoroutine(greenOrbDoor.Fall());
                    break;
                case "blue":
                    HasBlueOrb = true;
                    if (blueOrbDoor) blueOrbDoor.StartCoroutine(blueOrbDoor.Fall());
                    break;
                case "purple":
                    HasPurpleOrb = true;
                    if (purpleOrbDoor) purpleOrbDoor.StartCoroutine(purpleOrbDoor.Fall());
                    break;
                default:
                    Debug.LogWarning($"KeyColor {color} is missing or invalid. Proper format: red");
                    break;
            }
        }
    }
}