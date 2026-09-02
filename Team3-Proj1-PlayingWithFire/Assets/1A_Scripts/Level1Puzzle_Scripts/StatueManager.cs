using System.Collections;
using UnityEngine;

/// <summary>
/// Owns the actual fountain puzzle logic -- the 4 StatuePuzzle scripts just rotate themselves and
/// tell this when they're facing away. This decides what happens because of it.
/// </summary>
public class StatueManager : MonoBehaviour
{
    public static StatueManager Instance { get; private set; }

    [SerializeField] private GameObject mainPOI;
    [SerializeField] private GameObject keyBubble;
    [SerializeField] private Transform fountainWater;
    [SerializeField] private float waterLowerDistance = 3f;
    [SerializeField] private float waterLowerSpeed = 1f;
    [SerializeField] private AudioClip bubblePop;
    private AudioSource audioSource;
    private int statuesSolved;
    private bool canInteract;
    public bool CanInteract => canInteract;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if (mainPOI != null)
        {
            mainPOI.SetActive(false);
        }

        canInteract = false;
    }

    // Called by Keys.cs ONLY when the green key is picked up. Duct tape fix -- was a 3-key counter, kept double-firing early.
    public void UnlockStatues()
    {
        if (mainPOI != null)
        {
            mainPOI.SetActive(true);
        }

        canInteract = true;
    }

    // Called by a StatuePuzzle once it's actually facing away from the fountain.
    public void StatueSolved()
    {
        statuesSolved++;

        if (statuesSolved >= 4)
        {
            if (keyBubble != null)
            {
                keyBubble.SetActive(false);
                audioSource.PlayOneShot(bubblePop);
            }

            if (fountainWater != null)
            {
                StartCoroutine(LowerWater());
            }
        }
    }

    private IEnumerator LowerWater()
    {
        Vector3 start = fountainWater.position;
        Vector3 end = start - new Vector3(0f, waterLowerDistance, 0f);
        float duration = waterLowerDistance / waterLowerSpeed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            fountainWater.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fountainWater.position = end;
    }
}
