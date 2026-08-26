using UnityEngine;
/// <summary>
/// Small script meant to go on anything we want to... bob and spin.
/// Adjust values in inspector to fit whatever is appropriate for your needs.
/// </summary>
public class BobAndSpin : MonoBehaviour
{
    [Header("Bob")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Rotate")]
    [SerializeField] private float rotateSpeed = 30f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
        //
        //
        //
    }
}