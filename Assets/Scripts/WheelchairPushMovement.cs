// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;

// public class WheelchairPushMovement : MonoBehaviour
// {
//     public XRBaseInteractor leftHand;
//     public XRBaseInteractor rightHand;

//     public Transform leftHandle;
//     public Transform rightHandle;

//     public float forceMultiplier = 150f;

//     private Rigidbody rb;

//     private bool leftGrabbing = false;
//     private bool rightGrabbing = false;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     public void LeftGrab(bool grab)
//     {
//         leftGrabbing = grab;
//     }

//     public void RightGrab(bool grab)
//     {
//         rightGrabbing = grab;
//     }

//     void FixedUpdate()
//     {
//         if (leftGrabbing)
//         {
//             Vector3 handVel = leftHand.velocity;
//             rb.AddForce(transform.forward * handVel.z * forceMultiplier);
//             rb.AddTorque(transform.up * -handVel.x * forceMultiplier * 0.1f);
//         }

//         if (rightGrabbing)
//         {
//             Vector3 handVel = rightHand.velocity;
//             rb.AddForce(transform.forward * handVel.z * forceMultiplier);
//             rb.AddTorque(transform.up * handVel.x * forceMultiplier * 0.1f);
//         }
//     }
// }
