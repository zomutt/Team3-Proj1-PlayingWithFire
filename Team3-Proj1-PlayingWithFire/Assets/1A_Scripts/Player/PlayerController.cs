using UnityEngine;

/// <summary>
/// 
/// </summary>
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        // Ensures there is only one instance of the player in a scene.
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag("Player");
        if (playerObjects.Length > 1)
        {
            Debug.LogWarning("Multiple PlayerController instances found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }
    }
}
