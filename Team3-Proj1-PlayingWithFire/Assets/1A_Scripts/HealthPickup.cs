using UnityEngine;
using _1A_Scripts.Player;

public class HealthPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (PlayerCombat.Instance.PlayerHealth >= PlayerCombat.Instance.PlayerMaxHealth) return;

        int healAmount = Mathf.RoundToInt(PlayerCombat.Instance.PlayerMaxHealth * 0.25f);
        PlayerCombat.Instance.HealPlayer(healAmount);

        // collider has to live on the crystal, but the whole group (crystal + flame) should go.
        // Deactivated, not destroyed, so it can come back when the player respawns.
        if (transform.parent)
        {
            transform.parent.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
