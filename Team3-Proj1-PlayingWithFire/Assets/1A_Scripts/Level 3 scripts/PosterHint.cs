using UnityEngine;

public class PosterHint : MonoBehaviour
{
    [SerializeField] private GameObject posterUI; 

    private bool playerInRange;
    private bool hasBeenViewed;
    private bool isShowing;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (!isShowing && playerInRange && Input.GetKeyDown(KeyCode.E))
        {

            ShowPoster();
        }

        if (!isShowing && hasBeenViewed && Input.GetKeyDown(KeyCode.R))
        {
            ShowPoster();
        }

        if (isShowing && Input.GetMouseButtonDown(0))
        {
            HidePoster();
        }
    }

    private void ShowPoster()
    {
        isShowing = true;
        hasBeenViewed = true;
        posterUI.SetActive(true);
    }

    private void HidePoster()
    {
        isShowing = false;
        posterUI.SetActive(false);
    }
}
