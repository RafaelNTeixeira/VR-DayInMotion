using UnityEngine;

public class CircuitMover : MonoBehaviour
{
    [Header("Circuit Configuration")]
    public Transform waypointParent;

    [Header("Movement Settings")]
    public float speed = 25f;
    public float turnSpeed = 10f;
    public float arrivalThreshold = 0.1f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private Transform targetWaypoint;

    private bool isStopped = false; 

    void Start()
    {
        if (waypointParent == null) return;

        int childCount = waypointParent.childCount;
        waypoints = new Transform[childCount];

        for (int i = 0; i < childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }

        if (waypoints.Length > 0)
        {
            targetWaypoint = waypoints[0];
        }
    }

    void Update()
    {
        // If stopped, we simply return early.
        if (isStopped || waypoints == null || waypoints.Length == 0) return;

        Move();
        CheckWaypointDistance();
    }

    public void StopCar()
    {
        isStopped = true;
    }

    public void ResumeCar()
    {
        isStopped = false;
        
        Debug.Log("Car Resumed. Speed is: " + speed);
    }

    void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        Vector3 direction = targetWaypoint.position - transform.position;
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
        }
    }

    void CheckWaypointDistance()
    {
        if (Vector3.Distance(transform.position, targetWaypoint.position) < arrivalThreshold)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }

            targetWaypoint = waypoints[currentWaypointIndex];
        }
    }
}