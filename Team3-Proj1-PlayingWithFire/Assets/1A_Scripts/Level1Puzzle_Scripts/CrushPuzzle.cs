using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]

public class CrushPuzzle : FireReceiver
{
    private AudioSource audioSource;
    [SerializeField] AudioClip partySounds;     // Ambient.
    [SerializeField] AudioClip screams;         // Totally not a Wilhelm shriek.
    [SerializeField] AudioClip splash;          // Sploosh.

    [SerializeField] float burnTime = 3f;  // How long the object must be on fire before it falls.
    [SerializeField] float screamDelay; // Delay before the scream sound plays after the object is hit by fire.
    private float burnProgress;
    private bool isBurning;
    private bool canBurn;   // Can't kill a mob with fire and a pillar twice, now can you?
    private Rigidbody rb;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Initially, the object should not be affected by gravity.
    }

    private void Start()
    {
        if (partySounds != null)
        {
            audioSource.PlayOneShot(partySounds);
        }
    }

    // Implementation of the abstract method from FireReceiver
    public override void ReceiveFire()
    {
        if (isBurning || !canBurn)
        {
            return;
        }

        burnProgress += Time.deltaTime;

        if (burnProgress >= burnTime)
        {
            Burn();
            rb.useGravity = true;
            isBurning = true;
        }
    }

    private void Burn()
    {
        canBurn = false;

        // Start the scream delay coroutine
        StartCoroutine(ScreamDelay());

        if (splash != null)
        {
            audioSource.PlayOneShot(splash);
        }
    }

    private IEnumerator ScreamDelay()
    {
        yield return new WaitForSeconds(screamDelay);
        if (screams != null)
        {
            audioSource.PlayOneShot(screams);
        }
    }
}
