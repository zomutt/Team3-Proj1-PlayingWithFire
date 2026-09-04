using UnityEngine;

public class WaterProjectile : MonoBehaviour // attach to enemy
{
    [SerializeField] private GameObject waterOrbPrefab;
    [SerializeField] private Transform firePoint; // only if you want to make the point different than the object it's attached to

    [SerializeField] private int orbCount = 6;
    [SerializeField] private float orbSpeed = 8f;
    [SerializeField] private float orbLifetime = 4f; // in case orb doesn't hit anything that destroys it, it goes away after 4 seconds

    [SerializeField] private float fireInterval = 6f;

    private float fireTimer;

    private void Update()
    {
        fireTimer += Time.deltaTime;

        if (fireTimer >= fireInterval)
        {
            fireTimer = 0f;
            FireRing();
        } 
    } // fires as soon as the interval is over

    private void FireRing()
    {
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;

        for (int i = 0; i < orbCount; i++)
        {
            float angle = (360f / orbCount) * i;
            Vector3 direction = Quaternion.Euler(0f, angle, 0f) * transform.forward; // adjusts ring to fit the amount of orbs 

            GameObject orb = Instantiate(waterOrbPrefab, origin, Quaternion.LookRotation(direction));

            Rigidbody rb = orb.GetComponent<Rigidbody>();
            rb.linearVelocity = direction * orbSpeed;

            Destroy(orb, orbLifetime);
        }
    }
}
