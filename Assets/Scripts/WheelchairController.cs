using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Rigidbody wheelchairRigidbody;
    [SerializeField] private float pushForceMultiplier = 50f; 
    [SerializeField] private float maxSpeed = 3f;

    [Header("Direction Tuning")]
    [Tooltip("Flip these (1 or -1) if one wheel spins the wrong way.")]
    [SerializeField] private float leftFlip = 1f;
    [SerializeField] private float rightFlip = -1f; // Often one side needs to be inverted

    [Header("Wheel Joints")]
    [SerializeField] private ConfigurableJoint leftWheelJoint;
    [SerializeField] private ConfigurableJoint rightWheelJoint;

    void FixedUpdate()
    {
        ApplyWheelForce(leftWheelJoint, leftFlip);   
        ApplyWheelForce(rightWheelJoint, rightFlip); 

        LimitSpeed();
    }

    private void ApplyWheelForce(ConfigurableJoint wheelJoint, float sideMultiplier)
    {
        if (wheelJoint == null) return;

        Rigidbody wheelBody = wheelJoint.GetComponent<Rigidbody>();
        
        // 1. Calculate velocity relative to the WHEEL
        Vector3 localVel = wheelJoint.transform.InverseTransformDirection(wheelBody.angularVelocity);

        // 2. Get Speed on X Axis (Red)
        float rotationSpeed = localVel.x; 

        // 3. Apply Side Multiplier
        float adjustedSpeed = rotationSpeed * sideMultiplier;

        if (Mathf.Abs(adjustedSpeed) < 0.1f) return;

        // 4. Force Direction = Z Axis (Blue)
        Vector3 forceDirection = wheelchairRigidbody.transform.forward;

        // 5. Calculate Force
        Vector3 force = forceDirection * (adjustedSpeed * pushForceMultiplier * Time.fixedDeltaTime);

        // 6. Apply Force at Floor Level (Y-Axis Fix)
        Vector3 pushPoint = wheelJoint.transform.position;
        
        // Since Z is forward, Y is now UP. 
        // We set the push point's Y to the chair's floor position to prevent wheelies.
        pushPoint.y = wheelchairRigidbody.transform.position.y; 

        wheelchairRigidbody.AddForceAtPosition(force, pushPoint);
    }

    private void LimitSpeed()
    {
        if (wheelchairRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            wheelchairRigidbody.linearVelocity = wheelchairRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}