using _1A_Scripts.Managers;
using UnityEngine;

public class Note : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameObject poi;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (poi)
            poi.SetActive(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        if (audioSource && audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        UIController.Instance.note.gameObject.SetActive(true);

        if(poi)
            poi.SetActive(false);
    }
}
