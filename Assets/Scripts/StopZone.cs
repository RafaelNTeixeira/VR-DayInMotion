using UnityEngine;

public class StopZone : MonoBehaviour
{
    private CircuitMover trappedCar;
    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    void Update()
    {
        if (trappedCar != null && myCollider != null && !myCollider.enabled)
        {
            ReleaseCar();
        }
    }

    // ---------------------------------------------------------
    // STOP: When car enters
    // ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        // If we already have a car, ignore others to prevent logic conflicts
        if (trappedCar != null) return;

        GameObject hitObject = GetCarObject(other);

        if (hitObject != null && hitObject.CompareTag("Car"))
        {
            CircuitMover mover = hitObject.GetComponent<CircuitMover>();
            if (mover != null)
            {
                trappedCar = mover;
                mover.StopCar();
                Debug.Log("Car Stopped.");
            }
        }
    }

    // ---------------------------------------------------------
    // RESUME: Standard Physics Exit
    // ---------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        // If the exiting object is the car we are holding, release it
        GameObject hitObject = GetCarObject(other);
        
        if (trappedCar != null && hitObject == trappedCar.gameObject)
        {
            ReleaseCar();
        }
    }

    // ---------------------------------------------------------
    // RESUME: When GameObject or Script is Disabled
    // ---------------------------------------------------------
    private void OnDisable()
    {
        // This runs if you turn off the entire GameObject in the Inspector
        if (trappedCar != null)
        {
            ReleaseCar();
        }
    }

    // Helper to release the car safely
    private void ReleaseCar()
    {
        if (trappedCar != null)
        {
            trappedCar.ResumeCar();
            trappedCar = null; // Forget the car so we are ready for the next one
            Debug.Log("Car Resumed (Zone Disabled or Exited).");
        }
    }

    // ---------------------------------------------------------
    // HELPER FUNCTIONS
    // ---------------------------------------------------------
    private GameObject GetCarObject(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            return other.attachedRigidbody.gameObject;
        }
        return other.gameObject;
    }
}