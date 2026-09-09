using UnityEngine;
using _1A_Scripts.Level_3_scripts;

public class ValveLvl3 : MonoBehaviour
{
    [SerializeField] private float targetAngle = 180f;
    [SerializeField] private float rotationSpeed = 2f;
    //[SerializeField] private GameObject valve;
    [SerializeField] private GameObject poi;

    public bool IsTurned { get; private set; }

    private float currentAngle;
    private bool playerInRange;

    private void Start()
    {
        if (poi) poi.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
    }

    private void Update()
    {
        if (playerInRange && !IsTurned && Input.GetKeyDown(KeyCode.E))
        {
            Turn();
        }

        if (Mathf.Approximately(currentAngle, targetAngle)) return;
        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        gameObject.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
    }

    private void Turn()
    {
        IsTurned = true;

        if (poi) poi.SetActive(true);

        LevelThreePuzzleManager.Instance.IncreaseValveCount();
    }
}
