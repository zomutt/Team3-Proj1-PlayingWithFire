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
            // matches the parameters SoldierLadyAnim.controller actually reads
            animator.SetFloat("Speed", PlayerMovement.Instance.CurrentSpeed);
            animator.SetBool("isJumping", !PlayerMovement.Instance.IsGrounded);
        }
    }
}
