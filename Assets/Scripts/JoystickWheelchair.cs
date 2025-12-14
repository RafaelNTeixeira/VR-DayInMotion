using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class WheelchairJoystickDrive : MonoBehaviour
{
    [Header("Input Actions")]
    public InputActionReference leftMoveAction;

    [Header("Movement Settings")]
    public float forwardSpeed = 5f;
    public float acceleration = 5f;
    public float turnSpeed = 90f;
    
    [Header("Physics Settings")]
    public float flatGroundDrag = 5f;  // High drag to stop quickly on flat ground
    public float slopeDrag = 0f;       // Zero drag to slide fast on ramps
    public float slopeThreshold = 5f;  // Degrees to consider a "slope"
    public float groundCheckDist = 1.5f; // Length of raycast down (adjust to wheelchair height)

    private Rigidbody rb;
    private float currentSpeed = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        
        // IMPORTANT: Prevent the physics engine from "sleeping" the object when it moves slowly
        rb.sleepThreshold = 0f; 
    }

    private void OnEnable() => leftMoveAction?.action?.Enable();
    private void OnDisable() => leftMoveAction?.action?.Disable();

    private void FixedUpdate()
    {
        Vector2 input = leftMoveAction.action.ReadValue<Vector2>();
        float moveInput = input.y;
        float turnInput = input.x;

        // 1. Rotation (Always active)
        float yaw = turnInput * turnSpeed * Time.fixedDeltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, yaw, 0f));

        // 2. Check if we are driving or coasting
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            // --- DRIVING ---
            rb.linearDamping = 0f; // No drag while motor is on

            float targetSpeed = moveInput * forwardSpeed;
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
            
            Vector3 forward = transform.forward * currentSpeed;
            rb.linearVelocity = new Vector3(forward.x, rb.linearVelocity.y, forward.z);
        }
        else
        {
            // --- COASTING / SLIDING ---
            currentSpeed = 0f; // Reset motor logic

            // Check if we are on a slope
            if (IsOnSlope())
            {
                // We are on a ramp: Cut the brakes!
                rb.linearDamping = slopeDrag; 
            }
            else
            {
                // We are on flat ground: Apply brakes
                rb.linearDamping = flatGroundDrag;
            }
        }
    }

    private bool IsOnSlope()
    {
        // Raycast down to find the ground
        if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out RaycastHit hit, groundCheckDist))
        {
            // Calculate angle between the ground normal and straight UP
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle > slopeThreshold;
        }
        return false; // In the air, assume no slope logic needed
    }
    
    // Visualize the raycast in the editor to help debug
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 0.5f + Vector3.down * groundCheckDist);
    }
}