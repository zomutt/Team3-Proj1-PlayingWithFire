using UnityEngine;

namespace _1A_Scripts
{
    public class PlayerStart : MonoBehaviour
    {
        // This script is necessary because for some god forsaken reason the player just yeets herself into oblivion without it.
    
        public static PlayerStart Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }
        public void StartPlayerHere(GameObject player)
        {
            player.transform.position = gameObject.transform.position;
        }
    }
}
