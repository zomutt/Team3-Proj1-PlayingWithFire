using System.Collections;
using _1A_Scripts.Player;
using UnityEngine;

/// <summary>
/// Player enters trigger box (that this script is on) and that triggers water to rise. If the player doesn't get out of the trigger box in time, they die and respawn.
/// </summary>
public class RisingWaterTrap : MonoBehaviour
{
    [SerializeField] private Transform water;
    [SerializeField] private float riseSpeed = 1f;
    [SerializeField] private float riseHeight = 3f; // how high it climbs before it caps out
    [SerializeField] private float timeToEscape = 5f;

    private bool playerInside;
    private bool triggered;
    private Vector3 waterStartPosition;

    private void Awake()
    {
        waterStartPosition = water.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || triggered)
        {
            return;
        }

        triggered = true;
        playerInside = true;
        StartCoroutine(RiseWater());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    private IEnumerator RiseWater()
    {
        Vector3 targetPosition = waterStartPosition + Vector3.up * riseHeight;
        float elapsed = 0f;

        while (elapsed < timeToEscape)
        {
            if (!playerInside)
            {
                triggered = false;
                yield break; // They got out in time, stop rising and reset for next time.
            }

            water.position = Vector3.MoveTowards(water.position, targetPosition, riseSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        PlayerController.Instance.Respawn();
        water.position = waterStartPosition;
        triggered = false;
    }
}
