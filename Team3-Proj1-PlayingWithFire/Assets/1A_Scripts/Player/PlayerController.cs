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
        if (Instance)
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
        if (fadePanel)
        {
            fadePanel.SetActive(false);
        }
    }

    public void AddKey()
    {
        keysCollected++;
    }

    public void Respawn()
    {
        StartCoroutine(RespawnRoutine());
    }

    // One-way fade, no fade back in -- for when the scene is about to unload anyway (end of level).
    public IEnumerator FadeToBlack()
    {
        fadePanel.SetActive(true);

        Color color = fadeImage.color;
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
    }

    private IEnumerator RespawnRoutine()
    {
        fadePanel.SetActive(true); // it's off by default so it's not blocking the screen during normal gameplay

        PlayerMovement.Instance.ToggleMove();

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
        PlayerMovement.Instance.Teleport(respawnPoint.position);

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

        fadePanel.SetActive(false);

        PlayerMovement.Instance.ToggleMove();
    }
}
