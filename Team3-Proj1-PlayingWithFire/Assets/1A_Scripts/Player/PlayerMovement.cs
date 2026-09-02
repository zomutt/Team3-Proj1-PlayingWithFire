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

        [Header("Movement")]
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;

        [Header("Jump")]
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float groundCheckDistance = 1.1f;

        [Header("Look")]
        [SerializeField] private float mouseSensitivity = 0.1f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 80f;

        public bool IsMoving => canMove && moveInput.sqrMagnitude > 0.01f; // for Princess's animator to read

        private Rigidbody rb;
        private Vector2 moveInput;
        private Vector2 lookInput;
        private float pitch;
        private bool isSprinting;
        private bool isGrounded;
        private bool canMove;

        private void Awake()
        {
            Instance = this;

            rb = GetComponent<Rigidbody>();

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

            Move();
        }

        private void Update()
        {
            Look();
        }

        private void Look()
        {
            if (!canMove)
                return;

            // yaw turns the whole body (camera comes along since it's a child)
            transform.Rotate(Vector3.up, lookInput.x * mouseSensitivity);

            // pitch only tilts the camera, clamped so you can't flip past looking straight up/down
            pitch = Mathf.Clamp(pitch - lookInput.y * mouseSensitivity, minPitch, maxPitch);
            cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
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
            if (!canMove)
                return;

            // forward = wherever the camera's looking now, not wherever the body's facing -- flattened so looking up/down doesn't mess with speed
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camRight * moveInput.x + camForward * moveInput.y;

            float currentSpeed = moveSpeed;
            if (isSprinting)
            {
                currentSpeed = sprintSpeed;
            }

            Vector3 targetVelocity = moveDirection * currentSpeed;
            targetVelocity.y = rb.linearVelocity.y;
            rb.linearVelocity = targetVelocity;
        }

        // PlayerInput calls these on its own when you press stuff, don't wire them up manually
        private void OnMove(InputValue value)
        {
            moveInput = value.Get<Vector2>();
        }

        private void OnLook(InputValue value)
        {
            lookInput = value.Get<Vector2>();
        }

        private void OnSprint(InputValue value)
        {
            isSprinting = value.isPressed;
        }

        private void OnJump(InputValue value)
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            }
        }
    }
}