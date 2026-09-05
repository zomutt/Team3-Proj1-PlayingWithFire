using UnityEngine;
/// <summary>
/// Small script meant to go on anything we want to... bob.
/// Adjust values in inspector to fit whatever is appropriate for your needs.
/// </summary>
public class Bob : MonoBehaviour
{
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}
