using UnityEngine;

namespace _1A_Scripts
{
    /// <summary>
    /// Small script meant to go on anything we want to... spin.
    /// Adjust values in inspector to fit whatever is appropriate for your needs.
    /// </summary>
    public class Spin : MonoBehaviour
    {
        [SerializeField] private float rotateSpeed = 15f;

        private void Update()
        {
            transform.Rotate(Vector3.up * (rotateSpeed * Time.deltaTime));
        }
    }
}
