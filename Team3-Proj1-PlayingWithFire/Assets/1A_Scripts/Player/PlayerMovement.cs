using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

namespace _1A_Scripts.Player
{
    /// <summary>
    /// MOVEMENT SCRIPT. Nyooooooom.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement Instance;

        [Header("References")]
        [SerializeField] private Transform cameraTransform;

        [SerializeField] private Animator animator;//yanichanges

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;
        [Range(0f, 0.3f)] [SerializeField] private float rotationSmoothTime = 0.12f; // how fast the character turns to face movement direction

        [Header("Jump")]
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float groundCheckDistance = 1.1f;

        public bool IsMoving => canMove && moveInput.sqrMagnitude > 0.01f; // for Princess's animator to read
        public bool IsGrounded => isGrounded;
        public float CurrentSpeed => new Vector2(rb.linearVelocity.x, rb.linearVelocity.z).magnitude; // horizontal speed only, for the animator's blend

        private Rigidbody rb;
        private FootstepSounds footstepSounds;
        private Vector2 moveInput;
        private bool isSprinting;
        private bool isGrounded;
        private bool canMove;
        private float rotationVelocity; // SmoothDampAngle's running velocity state

        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            rb = GetComponent<Rigidbody>();
            footstepSounds = GetComponent<FootstepSounds>();

            // no tipping over, we handle rotation ourselves below
            rb.freezeRotation = true;

            // smooths the camera out so it's not choppy
            rb.interpolation = RigidbodyInterpolation.Interpolate;

            // locks + hides the cursor like every fps ever
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Start()
        {
            canMove = true;
        }

        private void FixedUpdate()
        {
            // short raycast down = are we touching the ground, so jump doesn't work midair
            isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

            if (isGrounded)
            {
                animator.SetBool("isJumping", false); // checks if landed
            }

            bool firing = Keyboard.current.fKey.isPressed;
            animator.SetBool("Fire", firing);

            Move();
        }

        public IEnumerator Teleport(Vector3 position)
        {
            rb.linearVelocity = Vector3.zero; // otherwise leftover momentum carries you right back off the spot

            // interpolation smooths rb.position changes into a glide -- turning it off and back on in the same
            // frame doesn't actually flush the buffer, so it needs to sit off for one real physics step first
            rb.interpolation = RigidbodyInterpolation.None;
            rb.position = position;
            transform.position = position;

            yield return new WaitForFixedUpdate();

            // if the respawn point's collider overlaps the floor even a little, physics shoves them apart here --
            // kill whatever velocity that added so it doesn't look like the player launching into the air
            rb.linearVelocity = Vector3.zero;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        public void ToggleMove()
        {
            canMove = !canMove;
        }

        private void Move()
        {
            //if (!canMove)
            //    return;

            //// forward = wherever the camera's looking now, not wherever the body's facing -- flattened so looking up/down doesn't mess with speed
            //Vector3 camForward = cameraTransform.forward;
            //Vector3 camRight = cameraTransform.right;
            //camForward.y = 0f;
            //camRight.y = 0f;
            //camForward.Normalize();
            //camRight.Normalize();

            //Vector3 moveDirection = camRight * moveInput.x + camForward * moveInput.y;
            //Debug.Log("MOVE: " + moveInput);

            //Yani change: Makes the player move
            if (!canMove)
                return;

            // Get the camera's forward and right directions
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            // Ignore looking up/down
            camForward.y = 0f;
            camRight.y = 0f;

            camForward.Normalize();
            camRight.Normalize();

            // WASD movement relative to camera
            Vector3 moveDirection =
                camRight * moveInput.x +
                camForward * moveInput.y;

            Turn(moveDirection);

            // Walking vs running
            float currentSpeed = isSprinting ? sprintSpeed : moveSpeed;

            Vector3 targetVelocity = moveDirection * currentSpeed;

            // Keep gravity/jumping
            targetVelocity.y = rb.linearVelocity.y;

            rb.linearVelocity = targetVelocity;



            // Animation speed
            float animationSpeed = 0f;

            if (moveInput.magnitude > 0.01f)
            {
                animationSpeed = isSprinting ? 2f : 1f;
            }

            animator.SetFloat("Speed", animationSpeed);
        }

        // same SmoothDampAngle-towards-move-direction approach as Unity's Starter Assets ThirdPersonController --
        // eases toward the target heading instead of turning at a constant rate, so it doesn't feel snappy
        private void Turn(Vector3 moveDirection)
        {
            if (moveDirection.sqrMagnitude < 0.0001f)
                return;

            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, targetAngle, ref rotationVelocity, rotationSmoothTime);
            rb.MoveRotation(Quaternion.Euler(0f, angle, 0f));
        }

        // PlayerInput calls these on its own when you press stuff, don't wire them up manually
        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
            
        }

        //private void OnSprint(InputValue value)
        //{
        //    isSprinting = value.isPressed;
        //    Debug.Log("Sprint: " + isSprinting);
        //}

        private void Update()
        {
            isSprinting = Keyboard.current.leftShiftKey.isPressed;
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                Debug.Log("SPACE DETECTED");
            }
        }

        private void OnJump(InputValue value)
        {
            //if (!value.isPressed)
            //    return;

            //if (isGrounded)
            //{
            //    rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);

            //    animator.SetBool("isJumping", true);//yani changes tells the animator when its jumping

            //}
            Debug.Log("JUMP: " + value.isPressed);

            if (!value.isPressed)
                return;

            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(
                    rb.linearVelocity.x,
                    jumpForce,
                    rb.linearVelocity.z
                );

                animator.SetBool("isJumping", true);
                footstepSounds?.PlayJumpSound();
            }

            }

        // tells animator when to use attack
        //private void OnUseFire(InputValue value)
        //{
        //    bool firing = value.isPressed;
        //    Debug.Log("FIRE: " + firing);
        //    animator.SetBool("Fire", value.isPressed);
            
        //}
    }
}