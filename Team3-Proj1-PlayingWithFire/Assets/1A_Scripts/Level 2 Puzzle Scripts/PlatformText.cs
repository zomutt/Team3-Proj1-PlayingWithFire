using UnityEngine;

namespace _1A_Scripts.Level_2_Puzzle_Scripts
{
        public class PlatformText : MonoBehaviour
        {
                [SerializeField] private GameObject platformText;

                private void Start()
                { 
                        platformText.SetActive(true);
                }

                private void OnTriggerEnter(Collider other)
                {
                        if (!other.CompareTag("Player"))
                        {
                                return;
                        }
                        else
                        {
                                platformText.SetActive(false);
                        }
                }
        }
}
