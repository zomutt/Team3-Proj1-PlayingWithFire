using System.Collections;
using UnityEngine;

/// <summary>
/// Goes on each of the 4 fountain statues. Only job here: rotate 45 degrees per fire hit and tell
/// StatueManager when it's actually facing away. StatueManager decides what happens because of that.
/// </summary>
public class StatuePuzzle : FireReceiver
{
    [SerializeField] private GameObject poiRing;
    [SerializeField] private float rotateHoldTime = 1.5f;   // How long to hold fire to trigger one 45 degree turn.
    [SerializeField] private float rotateDuration = 0.5f;   // How long the 45 degree turn itself takes to play out.
    [SerializeField] private float targetFacingAngle = 90f; // how far from the starting rotation counts as "facing away"

    private float fireProgress;
    private bool rotating;
    private bool solved;
    private float currentAngle;

    private void Start()
    {
        HidePOI();
    }

    public void ShowPOI()
    {
        if (poiRing != null)
        {
            poiRing.SetActive(true);
        }
    }

    public void HidePOI()
    {
        if (poiRing != null)
        {
            poiRing.SetActive(false);
        }
    }

    public override void ReceiveFire()
    {
        Debug.Log("StatuePuzzle received fire");
        if (rotating || solved)
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
        currentAngle = (currentAngle + 45f) % 360f; 
        rotating = false;

        if (Mathf.Approximately(currentAngle, targetFacingAngle))
        {
            solved = true;
            HidePOI();
            StatueManager.Instance.StatueSolved(); 
        }
    }
}
