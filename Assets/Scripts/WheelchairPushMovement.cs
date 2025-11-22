using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class WheelchairController : MonoBehaviour
{
    [Header("Assignments")]
    public XRBaseInteractable leftHandle;
    public XRBaseInteractable rightHandle;
    
    [Header("Settings")]
    public float pushPower = 200f; // Increased default slightly
    public float stopDrag = 5f;    // High drag when not holding (brakes)
    public float moveDrag = 0.5f;  // Low drag when pushing (gliding)

    private Rigidbody rb;
    private Transform leftHand, rightHand;
    private Vector3 lastLeftPos, lastRightPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.2f, 0); // Keeps it stable
        rb.linearDamping = stopDrag;
    }

    void OnEnable()
    {
        // Hook up events
        leftHandle.selectEntered.AddListener(OnGrabLeft);
        leftHandle.selectExited.AddListener(OnReleaseLeft);
        rightHandle.selectEntered.AddListener(OnGrabRight);
        rightHandle.selectExited.AddListener(OnReleaseRight);
    }

    void OnDisable()
    {
        leftHandle.selectEntered.RemoveListener(OnGrabLeft);
        leftHandle.selectExited.RemoveListener(OnReleaseLeft);
        rightHandle.selectEntered.RemoveListener(OnGrabRight);
        rightHandle.selectExited.RemoveListener(OnReleaseRight);
    }

    // --- Event Handlers ---
    private void OnGrabLeft(SelectEnterEventArgs args) 
    { 
        leftHand = args.interactorObject.transform;
        lastLeftPos = transform.InverseTransformPoint(leftHand.position);
        rb.linearDamping = moveDrag;
    }

    private void OnGrabRight(SelectEnterEventArgs args) 
    { 
        rightHand = args.interactorObject.transform;
        lastRightPos = transform.InverseTransformPoint(rightHand.position);
        rb.linearDamping = moveDrag;
    }

    private void OnReleaseLeft(SelectExitEventArgs args) { leftHand = null; CheckBrakes(); }
    private void OnReleaseRight(SelectExitEventArgs args) { rightHand = null; CheckBrakes(); }

    // --- Physics Loop ---
    void FixedUpdate()
    {
        if (leftHand) ApplyPush(leftHand, leftHandle.transform, ref lastLeftPos);
        if (rightHand) ApplyPush(rightHand, rightHandle.transform, ref lastRightPos);
    }

    private void ApplyPush(Transform hand, Transform handle, ref Vector3 lastPos)
    {
        Vector3 currentLocalPos = transform.InverseTransformPoint(hand.position);
        float pushDist = currentLocalPos.z - lastPos.z; // How far did hand move forward?

        // Only apply force if hand moved significantly
        if (Mathf.Abs(pushDist) > 0.001f)
        {
            // Calculate force: Direction * (Speed) * Power
            Vector3 force = transform.forward * (pushDist / Time.fixedDeltaTime) * pushPower;
            rb.AddForceAtPosition(force, handle.position);
        }

        lastPos = currentLocalPos;
    }

    private void CheckBrakes()
    {
        if (leftHand == null && rightHand == null)
            rb.linearDamping = stopDrag;
    }
}