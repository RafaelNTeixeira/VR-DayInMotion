using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Rigidbody wheelchairRigidbody;
    [SerializeField] private float pushForceMultiplier = 10f;
    [SerializeField] private float maxSpeed = 5f;

    [Header("Wheel Joints")]
    [SerializeField] private HingeJoint leftWheelJoint;
    [SerializeField] private HingeJoint rightWheelJoint;

    void FixedUpdate()
    {
        ApplyWheelForce(leftWheelJoint);
        ApplyWheelForce(rightWheelJoint);

        LimitSpeed();
    }

    private void ApplyWheelForce(HingeJoint wheel)
    {
        // 1. Get the speed the wheel is currently spinning (Degrees per second)
        // Note: You might need to invert this (-wheel.velocity) depending on your axis setup
        float angularVelocity = wheel.velocity;

        // 2. Ignore tiny jitters so the chair doesn't drift when standing still
        if (Mathf.Abs(angularVelocity) < 1f) return;

        // 3. Calculate force direction (Always forward relative to the chair)
        // We use the wheel's rotation speed to determine the MAGNITUDE of the push
        Vector3 forceDirection = wheelchairRigidbody.transform.forward;
        
        // 4. Calculate the actual force vector
        // Angular Velocity * Multiplier * Time (for smoothness)
        Vector3 force = forceDirection * (angularVelocity * pushForceMultiplier * Time.fixedDeltaTime);

        // 5. Apply the force at the specific location of the wheel
        // This creates "Torque" on the main body, allowing for turning!
        wheelchairRigidbody.AddForceAtPosition(force, wheel.transform.position);
    }

    private void LimitSpeed()
    {
        // Prevents the player from reaching supersonic speeds
        if (wheelchairRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            wheelchairRigidbody.linearVelocity = wheelchairRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}