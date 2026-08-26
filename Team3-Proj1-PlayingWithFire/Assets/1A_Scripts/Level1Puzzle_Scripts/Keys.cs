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


    private void Start()
    {
        gameObject.SetActive(true); // Ensure the key is active at the start of the game
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //gate.SetActive(false);

            if (pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);
            }

            gameObject.SetActive(false);

            if (gameObject.CompareTag("KeyCell"))
            {
                UIController.Instance.UpdateKeys("Red");
                wallCell.transform.position += Vector3.down * 5f;
                Debug.Log("Red key collected");
            }
            else if (gameObject.CompareTag("KeyFountain"))
            {
                UIController.Instance.UpdateKeys("Green");
                Debug.Log("Green key collected");
            }
            else if (gameObject.CompareTag("KeyCrush"))
            {
                UIController.Instance.UpdateKeys("Blue");
                wallCrush.transform.position += Vector3.down * 5f;
                Debug.Log("Blue key collected");
            }
            else if (gameObject.CompareTag("KeyRun"))  // Purple       
            {
                UIController.Instance.UpdateKeys("Purple");
                wallRun.transform.position += Vector3.down * 5f; 
                Debug.Log("Purple key collected");
            }
            else
            {
                Debug.LogWarning("Key has no recognized tag for performing any action.");
            }
        }
    }
}
