using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    [SerializeField] private GameObject fadePanel;
    [SerializeField] private float fadeDuration = 0.5f;
    [SerializeField] private Transform respawnPoint;     // Only works for one respawn point, but we can add more later if we want to get fancy

    private Image fadeImage;

    private int keysCollected;
    public int KeysCollected => keysCollected;

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

        fadeImage = fadePanel.GetComponent<Image>();
    }

    private void Start()
    {
        keysCollected = 0; // Initialize keys collected to 0 at the start of the game
    }

    public void AddKey()
    {
        keysCollected++;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }
    private IEnumerator RespawnRoutine()
    {
        fadePanel.SetActive(true); // it's off by default so it's not blocking the screen during normal gameplay

        Color color = fadeImage.color;

        // fade to black
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = elapsed / fadeDuration;
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f;
        fadeImage.color = color;

        // screen's fully black now, safe to teleport
        transform.position = respawnPoint.position;

        // fade back in
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a = 1f - (elapsed / fadeDuration);
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f;
        fadeImage.color = color;

        fadePanel.SetActive(false); // back off so it's not sitting there eating raycasts/draw calls
    }
}
