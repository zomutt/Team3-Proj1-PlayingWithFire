using System.Collections;
using UnityEngine;

/// <summary>
/// Puzzle 1 -- the ice door. Hold fire on it long enough and it melts open.
/// ***MATERIAL ON ICE DOOR MUST BE TRANSPARENT, NOT OPAQUE, OR THE FADE OUT WILL NOT WORK.***
/// </summary>
public class IceDoorPuzzle : FireReceiver
{
    [SerializeField] private float meltTime = 3f;   // How many seconds of sustained fire it takes to melt
    [SerializeField] private float fadeDuration = 1.5f;   // How long the fade-out takes once melting starts

    private Collider doorCollider;
    private Renderer doorRenderer;
    private float meltProgress;
    private bool isMelted;

    private void Start()
    {
        // The collider and renderer live on this same object, so no need to drag anything in the Inspector
        doorCollider = GetComponent<Collider>();
        doorRenderer = GetComponent<Renderer>();
    }

    public override void ReceiveFire()
    {
        if (isMelted)
        {
            return;
        }

        meltProgress += Time.deltaTime;

        if (meltProgress >= meltTime)
        {
            Melt();
        }
    }

    private void Melt()
    {
        isMelted = true;
        doorCollider.enabled = false; // Becomes passable right away even while the visual is still fading
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
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
