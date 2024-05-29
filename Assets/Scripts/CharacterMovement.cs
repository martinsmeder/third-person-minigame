using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public bool useCharacterForward = false;
    public bool lockToCameraForward = false;
    public float turnSpeed = 10f;
    public float speed = 20f;

    private float turnSpeedMultiplier;
    private float direction = 0f;
    private Vector3 targetDirection;
    private Vector2 input;
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
        input = movementValue.Get<Vector2>();
    }

    void OnLook(InputValue lookValue)
    {
        // ???
    }

    void FixedUpdate()
    {
        // Calculate direction based on input
        direction = input.magnitude;

        // Update target direction relative to the camera view
        UpdateTargetDirection();

        if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
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

        // Move the character
        Vector3 movement = targetDirection * speed * direction * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
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
            targetDirection = input.x * right + input.y * forward;
        }
        else
        {
            turnSpeedMultiplier = 0.2f;
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            forward.y = 0;

            // Get the right-facing direction of the referenceTransform
            Vector3 right = transform.TransformDirection(Vector3.right);
            targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
        }
    }
}