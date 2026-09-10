using System.Collections;
using _1A_Scripts.Level_3_scripts;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private float pullDuration = 1f;
    [SerializeField] private float cooldown = 1f; // adjusts these how you like
    [SerializeField] private LeverPuzzle puzzleManager;

    private bool playerInRange;
    private bool isUp = true;
    private bool isBusy;

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
        if (playerInRange && !isBusy && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PullLever());
        }
    }

    private IEnumerator PullLever()
    {
        isBusy = true;

        float startX = isUp ? 30f : -30f;
        float endX = isUp ? -30f : 30f; // probably need to change these as we put actual levers in
                                                // it's fiiiiiiiiiiiiiiine
        isUp = !isUp;

        Vector3 current = transform.localEulerAngles;
        float elapsed = 0f;

        while (elapsed < pullDuration)
        {
            float x = Mathf.Lerp(startX, endX, elapsed / pullDuration);
            transform.localRotation = Quaternion.Euler(x, current.y, current.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = Quaternion.Euler(endX, current.y, current.z);

        puzzleManager.CheckLevers();

        yield return new WaitForSeconds(cooldown);
        isBusy = false;
    }

    public bool IsUp()
    {
        return isUp;
    }
}