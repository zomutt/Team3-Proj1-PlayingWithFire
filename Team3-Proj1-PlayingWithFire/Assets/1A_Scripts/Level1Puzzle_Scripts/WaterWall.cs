using System.Collections;
using UnityEngine;

namespace _1A_Scripts.Level1Puzzle_Scripts
{
    public class WaterWall : MonoBehaviour
    {
        [SerializeField] private float lowerDistance = 10f;   // how far down it drops before it's gone
        [SerializeField] private float lowerSpeed = 2f;
        [SerializeField] private AudioClip waterSound; // sound to play when the water wall deactivates

        public IEnumerator Fall()
        {
            if (waterSound)
            {
                AudioSource.PlayClipAtPoint(waterSound, transform.position);
            }

            Vector3 start = transform.position;
            Vector3 end = start - new Vector3(0f, lowerDistance, 0f);
            var duration = lowerDistance / lowerSpeed;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(start, end, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = end;
            gameObject.SetActive(false);
        }
    }
}