using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        // Ensures there is only one instance of the player in a scene.
        if (Instance != null)
        {
            Debug.LogWarning("Multiple PlayerController instances found. Destroying duplicate.");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
