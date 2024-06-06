using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public float moveSpeed = 10f;
    public float jumpVelocity = 30f;
    public float jumpGravity = 5f;

    private float speedIncrement = 5f;
    private float incrementInterval = 5f;
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
        mainCamera = Camera.main; // Get and store main camera 
        rb = GetComponent<Rigidbody>(); // Get and store player's rigidbody
        StartCoroutine(IncreaseSpeedOverTime()); // Start the coroutine to increase speed over time
    }

    void OnMove(InputValue movementValue)
    {
        moveInput = movementValue.Get<Vector2>(); // Capture and store move input
    }

    void OnJump(InputValue jumpValue)
    {
        jumpInput = jumpValue.isPressed; // Capture and store jump input
    }

    void FixedUpdate()
    {
        UpdateTargetDirection();
        RotateCharacter();
        MoveCharacter();
        ApplyJumpGravity();
    }

    void Update()
    {
        HandleJump();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Ground") // If player hits the ground...
        {
            isJumping = false; // ... reset the isJumping flag
        }
    }

    private void UpdateTargetDirection()
    {
        // Check if character movement should be based on camera forward or character forward
        if (!useCharacterForward)
        {
            // Set the turn speed multiplier for camera-based movement
            turnSpeedMultiplier = 1f;

            // Get the forward direction relative to the camera and ignore the y-axis
            Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            // Get the right direction relative to the camera
            Vector3 right = mainCamera.transform.TransformDirection(Vector3.right);

            // Calculate the target direction based on movement input and camera directions
            targetDirection = moveInput.x * right + moveInput.y * forward;
        }
        else
        {
            // Set the turn speed multiplier for character-based movement
            turnSpeedMultiplier = 0.2f;

            // Get the forward direction relative to the character and ignore the y-axis
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            // Get the right direction relative to the character
            Vector3 right = transform.TransformDirection(Vector3.right);

            // Calculate the target direction based on movement input and character directions
            targetDirection = moveInput.x * right + Mathf.Abs(moveInput.y) * forward;
        }
    }

    private void RotateCharacter()
    {
        // Check if there is any movement input and if the target direction is significant
        if (moveInput != Vector2.zero && targetDirection.magnitude > 0.1f)
        {
            // Normalize the target direction to get the look direction
            Vector3 lookDirection = targetDirection.normalized;

            // Create a rotation based on the look direction
            freeRotation = Quaternion.LookRotation(lookDirection, transform.up);

            // Calculate the difference in rotation between the current rotation and the target rotation
            float differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;

            // Get the current y-axis rotation
            float eulerY = transform.eulerAngles.y;

            // If there is any difference in rotation, update the y-axis rotation to match the target rotation
            if (differenceRotation < 0 || differenceRotation > 0)
                eulerY = freeRotation.eulerAngles.y;

            // Create a new vector for the updated rotation
            var euler = new Vector3(0, eulerY, 0);

            // Smoothly interpolate the character's rotation towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }
    }

    private void MoveCharacter()
    {
        Vector3 movement = targetDirection * moveSpeed * moveInput.magnitude; // Create movement vector
        rb.AddForce(movement * Time.deltaTime, ForceMode.Impulse); // Apply force to rigidbody 
    }

    private void ApplyJumpGravity()
    {
        if (isJumping)
        {
            rb.AddForce(Physics.gravity * (jumpGravity - 1) * rb.mass); // Add custom gravity
        }
    }

    private void HandleJump()
    {
        if (jumpInput && !isJumping)
        {
            rb.AddForce(Vector3.up * jumpVelocity, ForceMode.Impulse); // Add jump force 
            isJumping = true;
        }

        jumpInput = false; // Reset jump input
    }

    public IEnumerator IncreaseSpeedOverTime()
    {
        while (true) // Infinite loop to keep the coroutine running
        {
            yield return new WaitForSeconds(incrementInterval); // Wait for the specified interval
            moveSpeed += speedIncrement; // Increase the movement speed
        }
    }
}