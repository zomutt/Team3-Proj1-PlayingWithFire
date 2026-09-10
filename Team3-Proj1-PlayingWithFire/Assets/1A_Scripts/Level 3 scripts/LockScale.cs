using UnityEngine;

namespace _1A_Scripts.Level_3_scripts
{
    // Brute-force fix for an animation clip that carries a bad baked-in scale curve.
    public class LockScale : MonoBehaviour
    {
        [SerializeField] private Vector3 lockedScale = Vector3.one;

        private void LateUpdate()
        {
            transform.localScale = lockedScale;
        }
    }
}
