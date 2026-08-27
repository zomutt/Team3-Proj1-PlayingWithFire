using UnityEngine;
using System.Collections;

public class WaterWall : MonoBehaviour
{
    [SerializeField] private float lowerDistance = 10f;   // how far down it drops before it's gone
    [SerializeField] private float lowerSpeed = 2f;
    [SerializeField] private AudioClip waterSound; // sound to play when the water wall deactivates

    public IEnumerator Fall()
    {
        if (waterSound != null)
        {
            AudioSource.PlayClipAtPoint(waterSound, transform.position);
        }

        Vector3 start = transform.position;
        Vector3 end = start - new Vector3(0f, lowerDistance, 0f);
        float duration = lowerDistance / lowerSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
        gameObject.SetActive(false);
    }
}