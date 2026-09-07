using UnityEngine;

namespace _1A_Scripts.Player
{
        public class HealPlayer : MonoBehaviour
        {
                [SerializeField] private int healAmt = 10;
                private bool hasPickedUp;   // Edge-case

                private void Start()
                {
                        hasPickedUp = false;
                }

                private void OnTriggerEnter(Collider other)
                {
                        if (!other.gameObject.CompareTag("Player") || hasPickedUp) return;
                
                        PlayerCombat.Instance.HealPlayer(healAmt);
                }
        }
}
