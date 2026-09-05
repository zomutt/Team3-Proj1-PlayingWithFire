using System.Collections;
using UnityEngine;

/// <summary>
/// This is a small script that will handle the use of the princess' fire spell.
/// NOTE: THE FIRE IS NOT AN ATTACK.
/// Considering the enemies are water, fire is her tool and her escape, not her weapon.
/// THIS SCRIPT GOES ON THE FIRE ITSELF PLS. :)
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class PlayerFire : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform fireOrigin; // Where the Raycast will originate from, and where the VFX will be spawned.

    [Header("Fire Stats")]
    [SerializeField] private float fireRange;
    [SerializeField] private float fireDuration; // How long she can hold the flame for before it fizzles.
    [SerializeField] private float fireCooldown; // How long she has to wait before she can use it again.

    private bool canFire;  // When she's off cooldown
    private bool isFiring; // Whether the button is currently held down
    private float fireTimer; // How long she's been continuously holding it down for

    [Header("Sound FX")]
    [SerializeField] private AudioClip fireWooshSound;     // When it is shot
    [SerializeField] private AudioClip fireFizzleSound;    // When she runs out of fire
    [SerializeField] private AudioClip fireCooldownSound;  // When she tries to use it while on cooldown
    private AudioSource audioSource;
    [SerializeField] private float fireWooshVolume;    // Adjustable. Sanity reasons.

    [Header("VFX")]
    [SerializeField] private ParticleSystem fireVFX_A;     // Both are needed here because the flame prefab has two separate particle systems.
    [SerializeField] private ParticleSystem fireVFX_B;     // If assigning, do NOT assign the flame parent prefab itself, you must go in and assign the two child particle systems instead.

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        canFire = true;
        fireVFX_A.Stop();  // Edge-case: in case it was left playing in the editor
        fireVFX_B.Stop();
    }

    private void Update()
    {
        // Old Input Manager instead of the new Input System -- straightforward press/held/release checks. Might change later. Might not.
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartFiring();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            StopFiring();
        }

        if (isFiring) // Only cast while the button is actually held.
        {
            fireTimer += Time.deltaTime;

            if (fireTimer >= fireDuration)
            {
                Fizzle();
            }
            else
            {
                bool didHit = Physics.Raycast(fireOrigin.position, fireOrigin.forward, out RaycastHit hit, fireRange);

                if (didHit)
                {
                    Debug.DrawLine(fireOrigin.position, hit.point, Color.red);

                    // Works for the ice door, pillar, torch, or anything else that inherits from FireReceiver -- this line never needs to change again. pLS.
                    FireReceiver receiver = hit.collider.GetComponentInParent<FireReceiver>();
                    if (receiver != null)
                    {
                        receiver.ReceiveFire();
                    }
                }
            }
        }
    }

    private void StartFiring()
    {
        if (canFire)
        {
            isFiring = true;
            fireTimer = 0f;
            fireVFX_A.Play();
            fireVFX_B.Play();
            if (audioSource != null && fireWooshSound != null)
            {
                audioSource.PlayOneShot(fireWooshSound, fireWooshVolume);
            }
            StartCoroutine(fireCD());
        }
        else
        {
            // Still on cooldown, just tell her no
            if (audioSource != null && fireCooldownSound != null)
            {
                audioSource.PlayOneShot(fireCooldownSound);
            }
        }
    }

    private void StopFiring()
    {
        isFiring = false;

        fireVFX_A.Stop();    // When you let go of the button, the fire stops spawning new particles.
        fireVFX_B.Stop();
    }

    private void Fizzle()
    {
        StopFiring(); // Ran out of fire while still holding the button -- cut it off, same as a release.

        if (audioSource != null && fireFizzleSound != null)
        {
            audioSource.PlayOneShot(fireFizzleSound);
        }
    }

    private IEnumerator fireCD()
    {
        canFire = false;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}
