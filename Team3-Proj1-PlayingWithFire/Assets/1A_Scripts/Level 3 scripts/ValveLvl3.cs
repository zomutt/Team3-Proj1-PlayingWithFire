using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    [RequireComponent(typeof(AudioSource))]
    public class ValveLvl3 : MonoBehaviour
    {
        [SerializeField] private float targetAngle = 180f;
        [SerializeField] private float rotationSpeed = 2f;
        //[SerializeField] private GameObject valve;
        [SerializeField] private GameObject poi;
    
    
        private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;

        public bool IsTurned { get; private set; }

        private float currentAngle;
        private bool playerInRange;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
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

            if (!IsTurned) return;
            if (Mathf.Approximately(currentAngle, targetAngle)) return;
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
            gameObject.transform.localEulerAngles = new Vector3(0f, 0f, currentAngle);
        }

        private void Turn()
        {
            audioSource.PlayOneShot(audioClip);     // Crrrreak.
            IsTurned = true;

            if (poi) poi.SetActive(true);

            LevelThreePuzzleManager.Instance.IncreaseValveCount();
        }
    }
}
