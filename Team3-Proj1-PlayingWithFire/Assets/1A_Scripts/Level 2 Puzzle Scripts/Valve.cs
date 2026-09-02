using System.Collections;
using UnityEngine;
using TMPro;
using _1A_Scripts.Level2Puzzles;

public class Valve : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI promptDisplay;
    [SerializeField] private string promptText = "E to rotate";
    [SerializeField] private int maxTurns = 2;
    [SerializeField] private float rotationStep = 45f;
    [SerializeField] private float rotationDuration = 0.3f;

    private int turnCount;
    private bool playerInRange;
    private bool isRotating;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (turnCount < maxTurns)
        {
            promptDisplay.text = promptText;
            promptDisplay.gameObject.SetActive(true);
        }
    } // detects player and shows text

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        promptDisplay.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && !isRotating && turnCount < maxTurns && Input.GetKeyDown(KeyCode.E))
        {
            Turn();
        }
    } // just makes sure the conditions are met for player to turn

    private void Turn()
    {
        StartCoroutine(RotateSmoothly());
    }

    private IEnumerator RotateSmoothly()
    {
        isRotating = true;

        Transform wheel = transform.GetChild(0);
        Quaternion start = wheel.localRotation;
        Quaternion end = start * Quaternion.Euler(0f, 0f, rotationStep);
        float elapsed = 0f;

        while (elapsed < rotationDuration)
        {
            wheel.localRotation = Quaternion.Slerp(start, end, elapsed / rotationDuration);  // Gradually rotates the valve over time. Existing code visually did not do anything. (Sorry)
            elapsed += Time.deltaTime;
            yield return null;
        }

        wheel.localRotation = end;
        isRotating = false;

        turnCount++;

        if (turnCount >= maxTurns)
        {
            promptDisplay.gameObject.SetActive(false);
        }

        LevelTwoPuzzleManager.Instance.CheckValves();
    } // sends info to puzzle manager once valve reaches the max turns and disables it

    public bool IsFullyTurned()
    {
        return turnCount >= maxTurns;
    }
}
