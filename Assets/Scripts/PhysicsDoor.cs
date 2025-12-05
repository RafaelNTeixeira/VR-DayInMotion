using UnityEngine;

// Class to manage a physics-based door that can be locked/unlocked by twisting a knob
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

    void Awake()
    {
        float currentSize = knobJoint.transform.lossyScale.x;
        
        knobJoint.connectedBody = GetComponent<Rigidbody>();
        knobJoint.transform.SetParent(null);
        
        // Apply that size back as a UNIFORM scale (e.g. 0.2, 0.2, 0.2)
        knobJoint.transform.localScale = new Vector3(currentSize, currentSize, currentSize);
    }

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

        // Check current Knob Rotation
        float knobAngle = Mathf.Abs(knobJoint.angle);

        // Check if Door is physically closed (approx 0 degrees)
        bool isDoorClosed = Mathf.Abs(doorJoint.angle) < 2f;

        // Logic:
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

    // Helper to change door lock state
    private void SetDoorState(bool locked)
    {
        if (isLocked == locked) return; // No change needed

        // DEBUG: Confirm the lock state actually switched
        Debug.Log($"<color=yellow>DOOR STATE CHANGED:</color> {(locked ? "LOCKED" : "UNLOCKED")}");

        isLocked = locked;
        doorJoint.limits = locked ? closedLimits : openLimits;
    }
}