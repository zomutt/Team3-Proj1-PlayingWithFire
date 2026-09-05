using _1A_Scripts.Player;
using UnityEngine;

namespace _1A_Scripts
{
    public class Respawn : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                PlayerController.Instance.HitRespawn();
            }
        }
    }
}
