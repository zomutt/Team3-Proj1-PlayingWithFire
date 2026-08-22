using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Began by: Christie Comer 5:00pm 08/22/2026
/// Contributed to by:
/// This is a small script that will handle the use of the princess' fire spell.
/// NOTE: THE FIRE IS NOT AN ATTACK.
/// Considering the enemies are water, fire is her tool and her escape, not her weapon.
/// </summary>
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

    [Header("Sound FX")]
    [SerializeField] private AudioClip fireWooshSound;    // When it is shot
    [SerializeField] private AudioClip fireFizzleSound;   // When she runs out of fire
    [SerializeField] private AudioClip fireCooldownSound; // When she tries to use it while on cooldown
    private AudioSource audioSource;
    // Sound.

    [Header("VFX")]
    [SerializeField] private ParticleSystem fireVFX;      // The visual effect of the fire spell. This lives as a child on the player prefab, and is toggled on/off when the spell is used.

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>(); 
    }
    private void Start()
    {
        canFire = true;
        fireVFX.Stop(); // Edge-case: in case it was left playing in the editor
    }

    private void Update()
    {
        if (isFiring) // Only cast while the button is actually held
        {
            bool didHit = Physics.Raycast(fireOrigin.position, fireOrigin.forward, out RaycastHit hit, fireRange);

            if (didHit)
            {
                // Ice door, pillar, torch, etc. will hook into this later
                Debug.DrawLine(fireOrigin.position, hit.point, Color.red);
            }
        }
    }

    private void OnUseFire(InputValue value)
    {
        isFiring = value.isPressed;

        if (isFiring)
        {
            if (canFire)
            {
                fireVFX.Play();
                if (audioSource != null && fireWooshSound != null)
                {
                    audioSource.PlayOneShot(fireWooshSound);
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
        else
        {
            fireVFX.Stop(); // When you let go of the button, the fire goes away immediately.
        }
    }

    private IEnumerator fireCD()
    {
        canFire = false;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }
}
