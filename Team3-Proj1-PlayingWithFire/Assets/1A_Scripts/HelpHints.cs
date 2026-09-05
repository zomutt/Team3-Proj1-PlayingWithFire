using UnityEngine;

namespace _1A_Scripts
{
    public class HelpHints : MonoBehaviour
    {
        public static HelpHints Instance;
        [SerializeField] private GameObject CollectImg;
        [SerializeField] private GameObject UseFireImg;
        [SerializeField] private GameObject AvoidImg;
        [SerializeField] private GameObject EndLevelImg;
        [SerializeField] private GameObject StatuesImg;
        [SerializeField] private GameObject[] allPanels;   // For activating and deactivating
        public enum HintPanels
        {
            Collect,
            UseFire,
            Avoid,
            Statues,
            EndLevel
        }

        private void Awake()
        {
            // Scene-local on purpose -- each level has its own hint images, and UIController already
            // re-finds whichever HelpHints belongs to the scene that just loaded.
            if (!Instance)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            DisableAll();
        }

        public void DisableAll()
        {
            foreach (var panel in allPanels)
            {
                panel.SetActive(false);
            }
        }
        
        public void DisplayHint(HintPanels hintType)
        {
            DisableAll();
            
            CollectImg.SetActive(hintType == HintPanels.Collect);
            UseFireImg.SetActive(hintType == HintPanels.UseFire);
            AvoidImg.SetActive(hintType == HintPanels.Avoid);
            StatuesImg.SetActive(hintType == HintPanels.Statues);
            EndLevelImg.SetActive(hintType == HintPanels.EndLevel);
        }
    }
}
