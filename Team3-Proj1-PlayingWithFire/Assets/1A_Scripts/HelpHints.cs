using UnityEngine;

namespace _1A_Scripts
{
    public class HelpHints : MonoBehaviour
    {
        public static HelpHints Instance;
        [Header("Panels")]
        [SerializeField] private GameObject CollectImg;
        [SerializeField] private GameObject UseFireImg;
        [SerializeField] private GameObject AvoidImg;
        [SerializeField] private GameObject EndLevelImg;
        [SerializeField] private GameObject StatuesImg;
        [SerializeField] private GameObject Enemy2Img;
        [SerializeField] private GameObject JumpImg;
        [SerializeField] private GameObject ValveImg;
        [SerializeField] private GameObject KingImg;
        
        [Header("Bulk")]
        [SerializeField] private GameObject[] allPanels;   // For activating and deactivating
        public enum HintPanels
        {
            Collect,
            UseFire,
            Avoid,
            Statues,
            EndLevel,
            Enemies2,
            Jump,
            Valve,
            King
        }

        private void Awake()
        {
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
            Enemy2Img.SetActive(hintType == HintPanels.Enemies2);
            JumpImg.SetActive(hintType == HintPanels.Jump);
            ValveImg.SetActive(hintType == HintPanels.Valve);
            KingImg.SetActive(hintType == HintPanels.King);
        }
    }
}
