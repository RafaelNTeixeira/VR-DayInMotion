using UnityEngine;

// Class to manage an automatic door that opens/closes when the player approaches
public class AutomaticDoor : MonoBehaviour
{
    [Header("Door Components")]
    [Tooltip("The Left Door HingeJoint")]
    [SerializeField] private HingeJoint leftDoor;
    
    [Tooltip("The Right Door HingeJoint")]
    [SerializeField] private HingeJoint rightDoor;

    [Header("Settings")]
    [Tooltip("Target angle when open (usually 90). Right door will automatically use negative.")]
    [SerializeField] private float openAngle = 90f;
    
    [Tooltip("How strong the door motor is. Higher = Faster/Snappier.")]
    [SerializeField] private float motorStrength = 50f;
    
    [Tooltip("How much air resistance. Higher = No wobbling at the end.")]
    [SerializeField] private float damper = 5f;

    [Header("Detection")]
    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
        // Initialize the joints so they are ready to move
        SetupJoint(leftDoor);
        SetupJoint(rightDoor);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object is the player
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            OpenDoors();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (string.IsNullOrEmpty(playerTag) || other.CompareTag(playerTag))
        {
            CloseDoors();
        }
    }

    private void OpenDoors()
    {
        // Left goes positive, Right goes negative (Mirrored)
        SetDoorTarget(leftDoor, -openAngle);
        SetDoorTarget(rightDoor, openAngle);
    }

    private void CloseDoors()
    {
        SetDoorTarget(leftDoor, 0f);
        SetDoorTarget(rightDoor, 0f);
    }

    // Helper to configure the joint physics
    private void SetupJoint(HingeJoint joint)
    {
        if (joint == null) return;

        joint.useSpring = true;
        
        JointSpring spring = joint.spring;
        spring.spring = motorStrength;
        spring.damper = damper;
        spring.targetPosition = 0f; // Start closed
        
        joint.spring = spring;
    }

    // Helper to move the door
    private void SetDoorTarget(HingeJoint joint, float targetAngle)
    {
        if (joint == null) return;

        JointSpring spring = joint.spring;
        spring.targetPosition = targetAngle;
        joint.spring = spring;
    }
}