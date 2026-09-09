using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    public class ProjectileManager : MonoBehaviour
    {
        // This script serves to choose which spawner will be activated at any given time.

        [SerializeField] private GameObject[] spawners;
        [SerializeField] private float fireInterval;
        private float fireTimer;


        private void Update()
        {
            if (spawners == null || spawners.Length == 0) return;
            fireTimer += Time.deltaTime;
        
            if (fireTimer >= fireInterval)
            {
                fireTimer = 0f;
                CalculateSpawner();
            } 
        }

        private void CalculateSpawner() // This decides which spawner should be randomly spawned from
        {
            if (spawners == null || spawners.Length == 0)
            {
                Debug.LogWarning("No projectile spawners found");
                return; // In case the array was never assigned
            }
            int chosenSpawner = Random.Range(0, spawners.Length);
            if (!spawners[chosenSpawner]) return;    // If this specific spawner dies for any reason

            WaterOrbSpawner spawnerScript = spawners[chosenSpawner].GetComponent<WaterOrbSpawner>();
            if (!spawnerScript) return;

            spawnerScript.FireRing();
        }
    }
}
