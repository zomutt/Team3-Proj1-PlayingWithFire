using System.Collections;
using _1A_Scripts.Managers;
using UnityEngine;

namespace _1A_Scripts.Player
{
    public class PlayerCombat : MonoBehaviour
    {
        public static PlayerCombat Instance;
        
        [SerializeField] private int playerDamage;
        public int PlayerDamage => playerDamage;
        
        [SerializeField] private float playerMaxHealth;
        public float PlayerMaxHealth => playerMaxHealth;
        
        [SerializeField] private float playerHealth;
        public float PlayerHealth => playerHealth;

        [SerializeField] private float playerIframe;
        public float PlayerIframe => playerIframe;

        private float currentIframeCD;
        private bool canTakeDamage;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            canTakeDamage = true;
        }
        
        private void Start()
        {
            playerHealth = playerMaxHealth;
            UIController.Instance.UpdateHealthDisplay();
        }
        
        public void HealPlayer(int healAmount)
        {
            if (playerHealth + healAmount > playerMaxHealth) return;

            playerHealth += healAmount;
            UIController.Instance.UpdateHealthDisplay();
        }

        // Player survives scene loads (DontDestroyOnLoad'd along with the rest of the Player object),
        // so a new playthrough needs this called or they'd start still hurt/dead from the last run.
        public void ResetHealth()
        {
            playerHealth = playerMaxHealth;
            canTakeDamage = true;
            UIController.Instance.UpdateHealthDisplay();
        }
        
        public void TakeDamage(float damageAmount)
        {
            if (!canTakeDamage) return;
            
            playerHealth -= damageAmount;
            StartCoroutine(Iframe());
            UIController.Instance.UpdateHealthDisplay();
        }

        private IEnumerator Iframe()
        {
            canTakeDamage = false;
            yield return new WaitForSeconds(playerIframe);
            canTakeDamage = true;
        }
    }
}
