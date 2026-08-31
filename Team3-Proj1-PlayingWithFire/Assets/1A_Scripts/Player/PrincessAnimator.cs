using UnityEngine;

namespace _1A_Scripts.Player
{
    [RequireComponent(typeof(Animator))]
    public class PrincessAnimator : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            animator.SetBool("IsWalking", PlayerMovement.Instance.IsMoving);
        }
    }
}
