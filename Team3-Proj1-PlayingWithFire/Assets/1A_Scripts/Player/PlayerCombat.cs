using System.Collections;
using _1A_Scripts.Managers;
using UnityEngine;

namespace _1A_Scripts.Player
{
    [RequireComponent(typeof(AudioSource))]
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

        [Header("Sound FX")]
        [SerializeField] private AudioClip hurtSound;
        [SerializeField] private float hurtVolume = 1f;
        [SerializeField] private AudioClip healSound;
        [SerializeField] private float healVolume = 1f;
        private AudioSource audioSource;

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
            audioSource = GetComponent<AudioSource>();
        }
        
        private void Start()
        {
            playerHealth = playerMaxHealth;
            UIController.Instance.UpdateHealthDisplay();
        }
        
        public void HealPlayer(int healAmount)
        {
            playerHealth += healAmount;
            if (playerHealth > playerMaxHealth)
            {
                playerHealth = playerMaxHealth;
            }

            if (audioSource != null && healSound != null)
            {
                audioSource.PlayOneShot(healSound, healVolume);
            }
            else
            {
                Debug.LogWarning("no heal sound assigned on PlayerCombat");
            }

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

            if (audioSource != null && hurtSound != null)
            {
                audioSource.PlayOneShot(hurtSound, hurtVolume);
            }
            else
            {
                Debug.LogWarning("no hurt sound assigned on PlayerCombat");
            }

            UIController.Instance.FlashHitPanel();

            if (playerHealth <= 0)
            {
                playerHealth = 0;
                UIController.Instance.UpdateHealthDisplay();
                PlayerController.Instance.Respawn(true);
                return;
            }

            UIController.Instance.UpdateHealthDisplay();
            StartCoroutine(Iframe());
        }

        // Called by PlayerController's respawn routine once the screen is faded to black,
        // so the heal happens off-screen instead of overwriting the 0-health display instantly.
        public void RespawnHeal()
        {
            playerHealth = playerMaxHealth * 0.25f;
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
