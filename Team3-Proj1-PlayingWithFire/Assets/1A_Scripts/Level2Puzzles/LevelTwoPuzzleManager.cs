using UnityEngine;

namespace _1A_Scripts.Level2Puzzles
{
    public class LevelTwoPuzzleManager : MonoBehaviour
    {
        private static LevelTwoPuzzleManager Instance;
        [Header("Misc")] 
        [SerializeField] private GameObject[] ActivateOnStart;
        [SerializeField] private GameObject[] DeactivateOnStart;

        [Header("Keys")] 
        [SerializeField] private GameObject keyRed;
        [SerializeField] private GameObject keyGreen;
        [SerializeField] private GameObject keyBlue;
        [SerializeField] private GameObject keyPurple;


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
    }
}
