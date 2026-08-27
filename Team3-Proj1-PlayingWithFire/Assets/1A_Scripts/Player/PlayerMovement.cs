using UnityEngine;
using UnityEngine.InputSystem;

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

    [Header("Look")]
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float pitchClamp = 85f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float groundCheckDistance = 1.1f;

    public float Pitch => pitch; // so ThirdPersonCamera can steal this instead of reading mouse input a second time

    private Rigidbody rb;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private bool isSprinting;
    private bool isGrounded;
    private float pitch;
    private float turnBuildup; // mouse movement stacks up here until FixedUpdate actually uses it
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

    private void Update()
    {
        // mouse only updates once a frame but FixedUpdate can run more (or less) than once a frame,
        // so if you apply mouse movement straight in FixedUpdate it stuttrs BAD - this fixes that.
        turnBuildup += lookInput.x * lookSensitivity;
        Look();
    }


    private void FixedUpdate()
    {
        // short raycast down = are we touching the ground, so jump doesn't work midair
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);

        Turn();
        Move();
    }

    private void Turn()
    {
        // MoveRotation instead of transform.Rotate bc rotating the rigidbody's transform directly
        // was the OG jitter bug (interpolation doesn't know about it if you don't). learned that one the hard way
        Quaternion turnAmount = Quaternion.Euler(0f, turnBuildup, 0f);
        rb.MoveRotation(rb.rotation * turnAmount);
        turnBuildup = 0f; // used it, dump it
    }

    private void Look()
    {
        // pitch just spins the camera, not the whole body, so we don't faceplant the capsule
        pitch -= lookInput.y * lookSensitivity;
        pitch = Mathf.Clamp(pitch, -pitchClamp, pitchClamp);
        cameraTransform.localEulerAngles = new Vector3(pitch, 0f, 0f);
    }

    public void Teleport(Vector3 position)
    {
        rb.linearVelocity = Vector3.zero; // otherwise leftover momentum carries you right back off the spot
        rb.position = position;
        transform.position = position;
    }

    public void ToggleMove()
    {
        if (canMove)
        {
            canMove = false;
        }
        else
            canMove = true;
    }

    private void Move()
    {
        if (!canMove)
            return;
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