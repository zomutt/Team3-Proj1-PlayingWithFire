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

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprinting;
    private bool isGrounded;
    private float cameraPitch;

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
        ApplyLook();
    }

   
    private void FixedUpdate()
    {
        // A short raycast straight down tells us if we're standing on something, so jump only works on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        ApplyMovement();
    }

    private void ApplyLook()
    {
        // Yaw rotates the body, pitch only rotates the camera so we don't tip the capsule over
        transform.Rotate(Vector3.up * (lookInput.x * lookSensitivity));

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

    // PlayerInput calls these On___ methods automatically when the action fires, don't need to hook them up yourself
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
