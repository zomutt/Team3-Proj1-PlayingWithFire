using System.Collections;
using UnityEngine;

/// <summary>
/// Goes on each key individually. It's 3am, this is the simple version. :^)))
/// </summary>
public class Keys : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private float gateLowerDistance = 6f;
    [SerializeField] private float gateLowerSpeed = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            // Hide + stop retriggering right away, but don't SetActive(false) the whole key yet --
            // that would kill the gate-lowering coroutine below before it finishes.
            Collider ownCollider = GetComponent<Collider>();
            if (ownCollider != null)
            {
                ownCollider.enabled = false;
            }

            Renderer ownRenderer = GetComponent<Renderer>();
            if (ownRenderer != null)
            {
                ownRenderer.enabled = false;
            }

            if (gameObject.CompareTag("KeyCell"))
            {
                GameManager.Instance.ObtainKey(0); // Red
                UIController.Instance.UpdateKeys(0);
            }
            else if (gameObject.CompareTag("KeyAvoid"))
            {
                GameManager.Instance.ObtainKey(1); // Blue
                UIController.Instance.UpdateKeys(1);
            }
            else if (gameObject.CompareTag("KeyCrush"))
            {
                GameManager.Instance.ObtainKey(2); // Green
                UIController.Instance.UpdateKeys(2);
            }
            else if (gameObject.CompareTag("KeyFountain"))
            {
                GameManager.Instance.ObtainKey(3); // Purple
                UIController.Instance.UpdateKeys(3);
            }

            if (gate != null)
            {
                StartCoroutine(LowerGateThenDisable());
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator LowerGateThenDisable()
    {
        Vector3 start = gate.transform.position;
        Vector3 end = start - new Vector3(0f, gateLowerDistance, 0f);
        float duration = gateLowerDistance / gateLowerSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            gate.transform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        gate.transform.position = end;
        gate.SetActive(false);
        gameObject.SetActive(false);
    }
}
