using UnityEngine;
using System.Collections;
public class WaterWall : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.5f;   // How long the fade-out takes once melting starts
    [SerializeField] private AudioClip waterSound; // Sound to play when the water wall is deactivated
    private Renderer doorRenderer;
    private void Start()
    {
        doorRenderer = GetComponent<Renderer>();
    }
    public IEnumerator FadeOut()
    {
        // Instead of the ice door just disappearing, it should fade instead.
        float elapsed = 0f;
        Color startColor = doorRenderer.material.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            float alpha = 1f - (elapsed / fadeDuration);
            Color fadedColor = startColor;
            fadedColor.a = alpha;
            doorRenderer.material.color = fadedColor;

            yield return null;
        }

        gameObject.SetActive(false);
    }
}
