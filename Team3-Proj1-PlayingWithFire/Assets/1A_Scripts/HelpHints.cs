using UnityEngine;

namespace _1A_Scripts
{
    public class HelpHints : MonoBehaviour
    {
        public static HelpHints Instance;
        [SerializeField] private GameObject CollectImg;
        [SerializeField] private GameObject UseFireImg;
        [SerializeField] private GameObject AvoidImg;
        [SerializeField] private GameObject[] allPanels;   // For activating and deactivating
        public enum HintPanels
        {
            Collect,
            UseFire,
            Avoid,
        }

        private void Awake()
        {
            if (!Instance)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }
        
        private void Start()
        {
            DisableAll();
        }

        private void DisableAll()
        {
            foreach (var panel in allPanels)
            {
                panel.SetActive(false);
            }
        }
        
        public void DisplayHint(HintPanels hintType)
        {
            DisableAll();
            switch (hintType)
            {
                case HintPanels.Collect:
                    CollectImg.SetActive(true);
                    break;
                case HintPanels.UseFire:
                    UseFireImg.SetActive(true);
                    break;
                case HintPanels.Avoid:
                    AvoidImg.SetActive(true);
                    break;
                default:
                    Debug.LogWarning("Unknown hint type. Use Collect, UseFire, or Avoid.");
                    break;
            }
        }
    }
}
