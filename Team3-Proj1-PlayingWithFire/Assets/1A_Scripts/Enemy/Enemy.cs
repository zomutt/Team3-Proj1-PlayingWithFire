using _1A_Scripts.Player;
using UnityEngine;

public class Enemy : FireReceiver
{
    public override void ReceiveFire()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController.Instance.Respawn();
        }
    }
}
