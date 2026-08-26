using UnityEngine;

/// <summary>
/// Goes on each key individually. It's 3am, this is the simple version. :^)))
/// </summary>
public class Keys : MonoBehaviour
{
    //[SerializeField] private GameObject gate;
    [SerializeField] private AudioClip pickupSound;

    [SerializeField] private GameObject wallCell;
    [SerializeField] private GameObject wallRun;
    //[SerializeField] private GameObject wallAvoid;
    [SerializeField] private GameObject wallCrush;

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
                GameManager.Instance.ObtainKey(0); // Red
                UIController.Instance.UpdateKeys(0);
            }
            else if (gameObject.CompareTag("KeyFountain"))
            {
                GameManager.Instance.ObtainKey(1); // Green
                UIController.Instance.UpdateKeys(1);
            }
            else if (gameObject.CompareTag("KeyCrush"))
            {
                GameManager.Instance.ObtainKey(2); // Blue
                UIController.Instance.UpdateKeys(2);
            }
            //else if (gameObject.CompareTag("KeyAvoid"))         // No need. It starts open.
            //{
            //    GameManager.Instance.ObtainKey(3); // 
            //    UIController.Instance.UpdateKeys(3);
            //}
        }
    }
}
