using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float moveAmount = 6.7f;    // total distance
    public float startDelay = 3f;    // wait before moving
    public float cooldown = 3f;      // wait after moving
    public float moveDuration = 3f;  // <-- NEW: time to move smoothly

    private bool isBusy = false;
    private bool moveUp = true;

    public Transform playerTransform; 

    public ElevatorDoorController elevatorDoorController;

    private void OnTriggerEnter(Collider other)
    {
        if (!isBusy && other.CompareTag("Player"))
        {
            playerTransform = other.transform;
        }
    }

    public IEnumerator MoveElevator(Transform player)
    {
        isBusy = true;

        // Wait before moving
        yield return new WaitForSeconds(startDelay);

        // Determine direction
        Vector3 direction = moveUp ? Vector3.up : Vector3.down;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * moveAmount;

        float elapsed = 0f;

        // Smooth movement loop
        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;

            // Move elevator
            transform.position = Vector3.Lerp(startPos, endPos, t);

            // Move player with elevator
            player.position = Vector3.Lerp(startPos, endPos, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Snap to final position
        transform.position = endPos;
        player.position = endPos;

        // Switch direction for next time
        moveUp = !moveUp;

        // Cooldown
        yield return new WaitForSeconds(cooldown);

        elevatorDoorController?.OpenDoors();

        isBusy = false;
    }
}
