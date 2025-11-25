using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WheelchairJoystickDrive : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference leftMoveAction;

    [Header("Settings")]
    public float forwardSpeed = 3f;
    public float acceleration = 5f;
    public float turnSpeed = 90f; // degrees per second

    private Rigidbody rb;
    private float currentSpeed = 0f;
    private float targetSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Prevent tipping
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void OnEnable()
    {
        leftMoveAction?.action?.Enable();
    }

    private void OnDisable()
    {
        leftMoveAction?.action?.Disable();
    }

    private void FixedUpdate()
    {
        // Read joystick input
        Vector2 input = leftMoveAction.action.ReadValue<Vector2>();
        float moveInput = input.y;   // forward/back
        float turnInput = input.x;   // left/right

        // Determine target speed
        targetSpeed = moveInput * forwardSpeed;

        // Smooth acceleration
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);

        // Apply forward/back movement
        Vector3 forward = transform.forward * currentSpeed;
        rb.linearVelocity = new Vector3(forward.x, rb.linearVelocity.y, forward.z);

        // Apply rotation (yaw) based on horizontal joystick
        float yaw = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, yaw, 0f));
    }
}
