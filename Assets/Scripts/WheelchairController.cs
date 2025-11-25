using UnityEngine;

public class WheelchairController : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private Rigidbody wheelchairRigidbody;
    [SerializeField] private float pushForceMultiplier = 50f; 
    [SerializeField] private float maxSpeed = 5f;

    [Header("Wheel Joints")]
    [SerializeField] private ConfigurableJoint leftWheelJoint;
    [SerializeField] private ConfigurableJoint rightWheelJoint;

    void FixedUpdate()
    {
        // 1.0f = Normal, -1.0f = Inverted
        // Since Left works, keep it 1.0f.
        // Since Right goes backwards, flip it with -1.0f.
        ApplyWheelForce(leftWheelJoint, 1.0f);   
        ApplyWheelForce(rightWheelJoint, -1.0f); 

        LimitSpeed();
    }

    // UPDATED: Added 'sideMultiplier' to handle the mirroring
    private void ApplyWheelForce(ConfigurableJoint wheelJoint, float sideMultiplier)
    {
        // Safety check
        if (wheelJoint == null) return;

        Rigidbody wheelBody = wheelJoint.GetComponent<Rigidbody>();

        Vector3 localVel = transform.InverseTransformDirection(wheelBody.angularVelocity);

        // 1. Get Speed
        float rotationSpeed = localVel.x;

        // 2. Apply Mirror Logic (The Fix)
        // This flips the calculation for the specific wheel passed in FixedUpdate
        float adjustedSpeed = rotationSpeed * sideMultiplier;

        if (localVel.magnitude > 0.1f) 
        {
             // Debug.Log($"{wheelJoint.name} Speed: {adjustedSpeed}");
        }

        // 3. Deadzone
        if (Mathf.Abs(adjustedSpeed) < 0.1f) return;

        // 4. Force Direction (Keeping your UP logic)
        Vector3 forceDirection = wheelchairRigidbody.transform.up;

        // 5. Calculate Force
        Vector3 force = forceDirection * (adjustedSpeed * pushForceMultiplier * Time.fixedDeltaTime);

        // 6. Apply at Floor Level
        Vector3 lowPoint = wheelJoint.transform.position;
        lowPoint.y = wheelchairRigidbody.transform.position.y; 

        wheelchairRigidbody.AddForceAtPosition(force, lowPoint);
    }

    private void LimitSpeed()
    {
        if (wheelchairRigidbody.linearVelocity.magnitude > maxSpeed)
        {
            wheelchairRigidbody.linearVelocity = wheelchairRigidbody.linearVelocity.normalized * maxSpeed;
        }
    }
}