using System.Collections;
using UnityEngine;

public class WaterWall : MonoBehaviour
{
    [SerializeField] private float lowerDistance = 3f;
    [SerializeField] private float lowerSpeed = 1f;

    public void LowerAndDisable()
    {
        StartCoroutine(LowerRoutine());
    }

    private IEnumerator LowerRoutine()
    {
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
