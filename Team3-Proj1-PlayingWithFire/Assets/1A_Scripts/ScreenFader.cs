using System;
using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader Instance { get; private set; }

    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void FadeToBlackAndBack(Action onBlackout = null)
    {
        StartCoroutine(FadeRoutine(onBlackout));
    }

    private IEnumerator FadeRoutine(Action onBlackout)
    {
        yield return Fade(0f, 1f);
        onBlackout?.Invoke(); // Player gets moved here, while the screen's fully black.
        yield return Fade(1f, 0f);
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        fadePanel.alpha = from;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadePanel.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = to;
    }
}
