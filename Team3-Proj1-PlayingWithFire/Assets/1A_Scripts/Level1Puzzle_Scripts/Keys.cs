using System.Collections;
using UnityEngine;

/// <summary>
/// Goes on each key individually. It's 3am, this is the simple version. :^)))
/// </summary>
public class Keys : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] private Transform wallCell;
    [SerializeField] private Transform wallRun;
    [SerializeField] private Transform wallCrush;

    private bool collected;

    private void Start()
    {
        gameObject.SetActive(true); // Ensure the key is active at the start of the game
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!collected && other.CompareTag("Player"))
        {
            collected = true; // guards against a second overlapping collider firing this twice for one pickup

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
                UIController.Instance.UpdateKeys("Red");

                var WaterWall = wallCell.GetComponent<WaterWall>();
                WaterWall.StartCoroutine(WaterWall.Fall());
                Debug.Log("Red key collected");
            }
            else if (gameObject.CompareTag("KeyFountain"))
            {
                UIController.Instance.UpdateKeys("Green");
                StatueManager.Instance.UnlockStatues(); // only the green key unlocks the statue puzzle
                Debug.Log("Green key collected");
            }
            else if (gameObject.CompareTag("KeyCrush"))
            {
                UIController.Instance.UpdateKeys("Blue");

                var WaterWall = wallCrush.GetComponent<WaterWall>();
                WaterWall.StartCoroutine(WaterWall.Fall());
                Debug.Log("Blue key collected");
            }
            else if (gameObject.CompareTag("KeyRun"))  // Purple
            {
                UIController.Instance.UpdateKeys("Purple");

                var WaterWall = wallRun.GetComponent<WaterWall>();
                WaterWall.StartCoroutine(WaterWall.Fall());
                Debug.Log("Purple key collected");
            }
            else
            {
                Debug.LogWarning("Key has no recognized tag for doing anything");
            }

            gameObject.SetActive(false);
        }
    }
}
