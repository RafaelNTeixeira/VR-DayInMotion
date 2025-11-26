using UnityEngine;

public class PhysicsDoor : MonoBehaviour
{
    [Header("Joints")]
    [Tooltip("The Hinge Joint on the Door Panel (Connects to Frame/World)")]
    [SerializeField] private HingeJoint doorJoint;

    [Tooltip("The Hinge Joint on the Handle/Knob (Connects to Door Panel)")]
    [SerializeField] private HingeJoint knobJoint;

    [Header("Settings")]
    [SerializeField] private float unlockAngle = 20f; // How far to twist knob to open
    [SerializeField] private float doorOpenAngle = 90f; // Max swing range

    private JointLimits closedLimits;
    private JointLimits openLimits;
    private bool isLocked = true;

    void Start()
    {
        // Define "Closed" (Door cannot move)
        closedLimits = new JointLimits();
        closedLimits.min = 0f;
        closedLimits.max = 0f; // Locked rigidly at 0

        // Define "Open" (Door can swing)
        openLimits = new JointLimits();
        openLimits.min = 0f;
        openLimits.max = doorOpenAngle;

        // Start Locked
        if (doorJoint != null)
        {
            doorJoint.limits = closedLimits;
            doorJoint.useLimits = true;
        }
    }

    void Update()
    {
        if (knobJoint == null || doorJoint == null) return;

        // 1. Check current Knob Rotation
        float knobAngle = Mathf.Abs(knobJoint.angle);

        // DEBUG 1: Is the knob actually turning?
        // If you grab and twist but see no logs, the grab interaction isn't rotating the joint.
        if (knobAngle > 1f) 
        {
            Debug.Log($"Knob twisting... Angle: {knobAngle:F1} / Unlock at: {unlockAngle}");
        }

        // 2. Check if Door is physically closed (approx 0 degrees)
        bool isDoorClosed = Mathf.Abs(doorJoint.angle) < 2f;

        // 3. Logic:
        // If Knob is twisted past threshold -> UNLOCK
        // If Knob is released AND Door is back at 0 -> LOCK
        
        if (knobAngle > unlockAngle)
        {
            SetDoorState(false); // Unlock
        }
        else if (isDoorClosed && knobAngle < 5f)
        {
            SetDoorState(true); // Lock (Latch)
        }
    }

    private void SetDoorState(bool locked)
    {
        if (isLocked == locked) return; // No change needed

        // DEBUG 2: Confirm the lock state actually switched
        Debug.Log($"<color=yellow>DOOR STATE CHANGED:</color> {(locked ? "LOCKED" : "UNLOCKED")}");

        isLocked = locked;
        doorJoint.limits = locked ? closedLimits : openLimits;
    }
}