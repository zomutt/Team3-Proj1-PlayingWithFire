using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// MOVEMENT SCRIPT: Handles movement and camera look via first person perspective.
/// The camera may be changed if you want to use a different perspective, but the script will not work without a Rigidbody and a Collider (e.g. Capsule Collider) on the same GameObject.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintSpeed;

    [Header("Look")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float pitchClamp = 85f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float groundCheckDistance = 1.1f;

    public float CameraPitch => cameraPitch; // ThirdPersonCamera reads this instead of us reading input twice

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprinting;
    private bool isGrounded;
    private float cameraPitch;
    private float yawAccumulator; // Mouse delta piles up here in Update, then FixedUpdate applies and clears it exactly once

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Stops physics from spinning us out if we clip a wall/ledge -- rotation is handled manually below instead
        rb.freezeRotation = true;

        // Smooths out the movement of the rigidbody for better camera movement
        rb.interpolation = RigidbodyInterpolation.Interpolate;  

        // Locks the cursor to the center of the screen and makes it invisible, just as in an FPS game. I will later implement a reticle and RayCasting.
        Cursor.lockState = CursorLockMode.Locked;  
    }

    private void Update()
    {
        // Mouse delta only arrives once per rendered frame -- bank it here so FixedUpdate
        // (which can fire 0, 1, or several times per frame) applies the full amount exactly once.
        yawAccumulator += lookInput.x * lookSensitivity;
        ApplyPitch();
    }


    private void FixedUpdate()
    {
        // A short raycast straight down tells us if we're standing on something, so jump only works on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        ApplyYaw();
        ApplyMovement();
    }

    private void ApplyYaw()
    {
        // Rotating the Rigidbody's body through MoveRotation instead of transform.Rotate, and doing it in
        // FixedUpdate instead of Update -- rotating a Rigidbody's transform directly outside of physics steps
        // is what was causing the original jitter, since the Rigidbody's interpolation doesn't know about it.
        Quaternion yawRotation = Quaternion.Euler(0f, yawAccumulator, 0f);
        rb.MoveRotation(rb.rotation * yawRotation);
        yawAccumulator = 0f;
    }

    private void ApplyPitch()
    {
        // Pitch only rotates the camera (a child transform, no physics involved) so we don't tip the capsule over
        cameraPitch -= lookInput.y * lookSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -pitchClamp, pitchClamp);
        cameraTransform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.right * moveInput.x + transform.forward * moveInput.y;

        float currentSpeed = moveSpeed;
        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
        }

        Vector3 targetVelocity = moveDirection * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;
    }

    // PlayerInput calls these On___ methods automatically when the action fires, no need to hook them up yourself
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
