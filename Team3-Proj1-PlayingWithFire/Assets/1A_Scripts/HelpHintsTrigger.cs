using UnityEngine;

namespace _1A_Scripts
{
    /// <summary>
    /// Helper script that only serves to communicate with HelpHints what hint is needed and when.
    /// </summary>
    public class HelpHintsTrigger : MonoBehaviour
    {
        [SerializeField] private HelpHints.HintPanels hintType;       // This directly references the enum so that we can use a dropdown yaaaaay :D

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            
            HelpHints.Instance.DisplayHint(hintType);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            HelpHints.Instance.DisableAll();
        }
    }
}
