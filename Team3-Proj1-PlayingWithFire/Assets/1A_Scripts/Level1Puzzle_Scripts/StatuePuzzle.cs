using System.Collections;
using UnityEngine;

/// <summary>
/// Goes on each of the 4 fountain statues. Fire rotates it 45 degrees at a time, needs two hits (90 total)
/// to face away from the fountain. Once all 4 statues are done, the fountain water fades and the purple key unlocks.
/// </summary>
public class StatuePuzzle : FireReceiver
{
    [SerializeField] private GameObject poiRing;
    [SerializeField] private GameObject purpleKey;
    [SerializeField] private Renderer fountainWater;
    [SerializeField] private float rotateHoldTime = 1.5f; // How long to hold fire to trigger one 45 degree turn.
    [SerializeField] private float rotateDuration = 0.5f;  // How long the 45 degree turn itself takes to play out.
    [SerializeField] private float waterFadeDuration = 2f;

    private static int statuesSolved;

    private float fireProgress;
    private bool rotating;
    private int turnsCompleted;

    private void Start()
    {
        if (poiRing != null)
        {
            poiRing.SetActive(false); // GameManager turns this on once the player has 3 of the 4 keys.
        }
    }

    public override void ReceiveFire()
    {
        if (rotating || turnsCompleted >= 2)
        {
            return;
        }

        fireProgress += Time.deltaTime;

        if (fireProgress >= rotateHoldTime)
        {
            fireProgress = 0f;
            StartCoroutine(RotateStatue());
        }
    }

    private IEnumerator RotateStatue()
    {
        rotating = true;

        Quaternion start = transform.rotation;
        Quaternion end = start * Quaternion.Euler(0f, 45f, 0f);
        float elapsed = 0f;

        while (elapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(start, end, elapsed / rotateDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = end;
        turnsCompleted++;
        rotating = false;

        if (turnsCompleted >= 2)
        {
            if (poiRing != null)
            {
                poiRing.SetActive(false);
            }

            statuesSolved++;

            if (statuesSolved >= 4)
            {
                if (purpleKey != null)
                {
                    purpleKey.SetActive(true);
                }

                if (fountainWater != null)
                {
                    StartCoroutine(FadeWater());
                }
            }
        }
    }

    private IEnumerator FadeWater()
    {
        Color startColor = fountainWater.material.color;
        float elapsed = 0f;

        while (elapsed < waterFadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = 1f - (elapsed / waterFadeDuration);
            Color fadedColor = startColor;
            fadedColor.a = alpha;
            fountainWater.material.color = fadedColor;
            yield return null;
        }
    }
}
