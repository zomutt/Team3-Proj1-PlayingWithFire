using UnityEngine;
using System.Collections;

/// <summary>
/// This lives on LoosePillar.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class CrushPuzzle : FireReceiver
{
    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] AudioClip partySounds;     // Ambient.
    [SerializeField] AudioClip screams;         // Totally not a Wilhelm shriek.
    [SerializeField] AudioClip splash;          // Sploosh.
    [SerializeField] private GameObject[] enemies;    // The fellas in the pool.
    [SerializeField] private GameObject poiRing;

    [SerializeField] float burnTime = 3f;  // How long the object must be on fire before it falls.
    [SerializeField] float screamDelay; // Delay before the scream sound plays after the object is hit by fire.
    private float burnProgress;
    private bool isBurning;
    private bool canBurn = true;   // Can't kill a mob with fire and a pillar twice, now can you?

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
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
            isBurning = true;
        }
    }

    private void Burn()
    {
        canBurn = false;
        audioSource.Stop();
        animator.SetTrigger("Fall");
        poiRing.SetActive(false);

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

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
