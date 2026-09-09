using UnityEngine;

public class EndNPC : MonoBehaviour
{
    // Very small script because she mostly depends on the existing HelpHintsTrigger.cs script
    
    
    private AudioSource audioSource;
    [SerializeField] private AudioClip clip;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        audioSource.PlayOneShot(clip);
    }
}
