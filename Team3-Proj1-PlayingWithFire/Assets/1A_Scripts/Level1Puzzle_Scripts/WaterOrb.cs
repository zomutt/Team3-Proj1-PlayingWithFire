using _1A_Scripts.Player;
using UnityEngine;

public class WaterOrb : MonoBehaviour
{
    [SerializeField] private float damage = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerCombat.Instance.TakeDamage(damage);
        }

        if (collision.gameObject.CompareTag("Level3") || collision.gameObject.CompareTag("Player")) // attach the level 3 tag to anything you want the orb to be destroyed
                                                                                                    // once it collides with it.
        {
            Destroy(gameObject);
        }
    }
}
