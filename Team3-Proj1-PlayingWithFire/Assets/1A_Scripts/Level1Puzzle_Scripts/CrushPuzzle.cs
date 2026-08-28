using UnityEngine;
using System.Collections;
using _1A_Scripts;

/// <summary>
/// This lives on LoosePillar.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]

public class CrushPuzzle : FireReceiver
{
    public bool HasFallen { get; private set; }

    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] AudioClip partySounds;     // Ambient.
    [SerializeField] AudioClip screams;         // Totally not a Wilhelm shriek.
    [SerializeField] AudioClip splash;          // Sploosh.
    [SerializeField] private GameObject[] enemies;    // The fellas in the pool.
    [SerializeField] private GameObject poiRing;
    [SerializeField] private Collider killBox;

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
        HasFallen = true;
        audioSource.Stop();
        animator.SetTrigger("Fall");
        poiRing.SetActive(false);
        LevelOnePuzzleManager.Instance.ActivateKey("green");

        if (killBox != null)
        {
            killBox.enabled = false;
        }

        // Start the scream delay coroutine
        StartCoroutine(ScreamDelay());

        if (splash != null)
        {
            audioSource.PlayOneShot(splash);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!HasFallen)
            {
                GameManager.Instance.RespawnPlayer();
            }

            return;
        }

        // Kill enemies on contact too, not just on the scream delay -- whichever gets them first.
        foreach (GameObject enemy in enemies)
        {
            if (other.gameObject == enemy)
            {
                Destroy(enemy);
                break;
            }
        }
    }

    private IEnumerator ScreamDelay()
    {
        yield return new WaitForSeconds(screamDelay);
        if (screams != null)
        {
            audioSource.Stop();    // Makes sure partySounds stop
            audioSource.PlayOneShot(screams);  
        }

        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }
    }
}
