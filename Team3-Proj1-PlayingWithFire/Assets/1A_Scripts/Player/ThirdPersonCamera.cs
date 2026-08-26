using UnityEngine;

/// <summary>
/// Goes on the Main Camera. This camera is NOT parented under the player anymore -- it's its own
/// object that follows the player around from behind, reading the pitch PlayerMovement already tracks.
/// </summary>
public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private float distance = 4f;
    [SerializeField] private float pivotHeight = 1.6f; // Roughly shoulder/head height on the player.
    [SerializeField] private float followSpeed = 12f;

    private void LateUpdate()
    {
        if (player == null || playerMovement == null)
        {
            return;
        }

        Vector3 pivot = player.position + Vector3.up * pivotHeight;
        Quaternion rotation = Quaternion.Euler(playerMovement.CameraPitch, player.eulerAngles.y, 0f);
        Vector3 desiredPosition = pivot - (rotation * Vector3.forward * distance);

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.rotation = rotation;
    }
}
