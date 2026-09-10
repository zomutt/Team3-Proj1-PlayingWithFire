using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    // Cosmetic only -- just flies toward the boss for the visual effect. Doesn't trigger anything.
    public class FlyToBoss : MonoBehaviour
    {
        [SerializeField] private Transform boss;
        [SerializeField] private float speed = 5f;

        private void Update()
        {
            if (!LeverPuzzle.LeversSolved) return;
            if (!boss) return;

            transform.position = Vector3.MoveTowards(transform.position, boss.position, speed * Time.deltaTime);
        }
    }
}
