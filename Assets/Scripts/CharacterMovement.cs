using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public float speed = 20f;
    public float jumpVelocity = 30f;
    public float jumpGravity = 5f; 

    private float turnSpeedMultiplier;
    private Vector2 moveInput;
    private bool jumpInput;
    private bool isJumping = false;
    private Vector3 targetDirection;
    private Quaternion freeRotation;
    private Camera mainCamera;
    private Rigidbody rb;

    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>();
    }

    void OnJump(InputValue jumpValue)
    {
        jumpInput = jumpValue.isPressed;
    }

    void FixedUpdate()
    {
        // Calculate direction based on input
        float direction = moveInput.magnitude;

        // Update target direction relative to the camera view
        UpdateTargetDirection();

        if (moveInput != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            Vector3 lookDirection = targetDirection.normalized;
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            float differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            float eulerY = transform.eulerAngles.y;

            if (differenceRotation < 0 || differenceRotation > 0)
                eulerY = freeRotation.eulerAngles.y;

            var euler = new Vector3(0, eulerY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }

        // Move the character using AddForce
        Vector3 movement = targetDirection * speed * direction;
        rb.AddForce(movement * Time.deltaTime, ForceMode.Impulse);

        // Apply custom gravity to player when jumping
        if (isJumping)
        {
            rb.AddForce(Physics.gravity * (jumpGravity - 1) * rb.mass);
        }
    }

    void Update()
    {
        if (jumpInput && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse);
            isJumping = true;
            jumpInput = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground")
        {
            isJumping = false;
        }
    }

    public virtual void UpdateTargetDirection()
    {
        if (!useCharacterForward)
        {
            turnSpeedMultiplier = 1f;
            Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            // Get the right-facing direction of the referenceTransform
            Vector3 right = mainCamera.transform.TransformDirection(Vector3.right);

            // Determine the direction the player will face based on input and the referenceTransform's right and forward directions
            targetDirection = moveInput.x * right + moveInput.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            // Get the right-facing direction of the referenceTransform
            Vector3 right = transform.TransformDirection(Vector3.right);
            targetDirection = moveInput.x * right + Mathf.Abs(moveInput.y) * forward;
        }
    }
}