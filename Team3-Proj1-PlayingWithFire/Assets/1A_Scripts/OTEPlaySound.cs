using System.Collections;
using UnityEngine;

/// <summary>
/// This is a very small interchangable script that can be put on anything that plays a sound when the player touches it.
/// </summary>

[RequireComponent(typeof(AudioSource))] 
public class OTEPlaySound : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip soundToPlay;
    [SerializeField] private bool canBeRepeated;
    private bool hasPlayedOnce;
    private bool isPlaying;    // The sound cannot be played if it is already playing


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")     // Has to be player, has to have a sound to play, can't already be playing, and if it has played once then it can only play again if canBeRepeated is true.
            && (!hasPlayedOnce || canBeRepeated) 
            && soundToPlay != null
            && !isPlaying)  
        {
            audioSource.Stop();    // In case the sound is already playing
            audioSource.PlayOneShot(soundToPlay);

            StartCoroutine(SoundCooldown());
            hasPlayedOnce = true;
        }
    }

    private IEnumerator SoundCooldown()
    {
        isPlaying = true;
        yield return new WaitForSeconds(soundToPlay.length);
        isPlaying = false;
    }
}

