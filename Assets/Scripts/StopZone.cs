using UnityEngine;
using System.Collections.Generic; // Required for using Lists

public class StopZone : MonoBehaviour
{
    // Changed from a single variable to a List to hold multiple cars
    private List<CircuitMover> trappedCars = new List<CircuitMover>();
    private Collider myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider>();
    }

    void Update()
    {
        // If the collider is disabled but the script is running, release everyone
        if (myCollider != null && !myCollider.enabled && trappedCars.Count > 0)
        {
            ReleaseAllCars();
        }
    }

    // ---------------------------------------------------------
    // STOP: When car enters
    // ---------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        // We no longer return if trappedCars is not null. We accept everyone.

        GameObject hitObject = GetCarObject(other);

        if (hitObject != null && hitObject.CompareTag("Car"))
        {
            CircuitMover mover = hitObject.GetComponent<CircuitMover>();
            
            // Check if mover exists AND is not already in our list (to avoid duplicates)
            if (mover != null && !trappedCars.Contains(mover))
            {
                trappedCars.Add(mover); // Add to the list
                mover.StopCar();
                Debug.Log($"Car Stopped. Total in zone: {trappedCars.Count}");
            }
        }
    }

    // ---------------------------------------------------------
    // RESUME: Standard Physics Exit
    // ---------------------------------------------------------
    private void OnTriggerExit(Collider other)
    {
        GameObject hitObject = GetCarObject(other);
        
        if (hitObject != null)
        {
            CircuitMover mover = hitObject.GetComponent<CircuitMover>();

            // If the exiting object is in our list, release it and remove it
            if (mover != null && trappedCars.Contains(mover))
            {
                mover.ResumeCar();
                trappedCars.Remove(mover);
                Debug.Log("Car Exited and Resumed.");
            }
        }
    }

    // ---------------------------------------------------------
    // RESUME: When GameObject or Script is Disabled
    // ---------------------------------------------------------
    private void OnDisable()
    {
        ReleaseAllCars();
    }

    // Helper to release ALL cars safely
    private void ReleaseAllCars()
    {
        // Loop through every car in the list
        foreach (CircuitMover car in trappedCars)
        {
            // Check for null in case a car was destroyed while stopped
            if (car != null)
            {
                car.ResumeCar();
            }
        }

        // Clear the list so it's empty
        trappedCars.Clear();
        Debug.Log("All Cars Released.");
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