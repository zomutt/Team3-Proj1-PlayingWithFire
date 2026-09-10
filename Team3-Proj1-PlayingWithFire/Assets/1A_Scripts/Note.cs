using _1A_Scripts.Managers;
using UnityEngine;

namespace _1A_Scripts
{
    public class Note : MonoBehaviour
    {
        private AudioSource audioSource;
        [SerializeField] private AudioClip audioClip;
        [SerializeField] private GameObject poi;
        [SerializeField] private GameObject blueOrb;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (poi)
                poi.SetActive(true);
        
            if (blueOrb)
                blueOrb.SetActive(false);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;

            if (audioSource && audioClip)
            {
                audioSource.PlayOneShot(audioClip);
            }

            if (UIController.Instance)
            {
                UIController.Instance.note.gameObject.SetActive(true);
            }

            if(poi)
                poi.SetActive(false);

            if (blueOrb)
                blueOrb.SetActive(true);
        }
    }
}
