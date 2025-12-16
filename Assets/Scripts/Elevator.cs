using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float moveAmount = 6.7f;
    public float startDelay = 3f;
    public float cooldown = 3f;
    public float moveDuration = 3f;

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

        yield return new WaitForSeconds(startDelay);

        // --- 1. DISABLE PHYSICS (So we can clip through ceilings) ---
        CharacterController playerController = player.GetComponent<CharacterController>();
        Collider playerCollider = player.GetComponent<Collider>();
        Rigidbody playerRb = player.GetComponent<Rigidbody>();

        if (playerController != null) playerController.enabled = false;
        else if (playerCollider != null) playerCollider.enabled = false;

        bool wasKinematic = false;
        if (playerRb != null) 
        {
            wasKinematic = playerRb.isKinematic;
            playerRb.isKinematic = true;
        }

        // --- 2. CALCULATE OFFSET (Crucial Fix) ---
        // We remember exactly where the player is standing relative to the elevator center.
        // This prevents us from snapping them into the floor mesh later.
        Vector3 playerOffset = player.position - transform.position;

        Vector3 direction = moveUp ? Vector3.up : Vector3.down;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction * moveAmount;

        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            float t = elapsed / moveDuration;
            
            // Optional: Smooth movement
            // t = t * t * (3f - 2f * t);

            Vector3 currentElevatorPos = Vector3.Lerp(startPos, endPos, t);

            // Move Elevator
            transform.position = currentElevatorPos;

            // Move Player using the OFFSET
            // We apply the elevator's new position, plus the distance they were standing apart.
            player.position = currentElevatorPos + playerOffset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Final Snap
        transform.position = endPos;
        
        // Snap player using the offset (Safe landing)
        player.position = endPos + playerOffset;

        // --- 3. RE-ENABLE PHYSICS ---
        // Now that we are safely at the destination (and not buried in the floor), 
        // we turn physics back on.
        if (playerController != null) playerController.enabled = true;
        else if (playerCollider != null) playerCollider.enabled = true;

        if (playerRb != null) playerRb.isKinematic = wasKinematic;

        moveUp = !moveUp;

        yield return new WaitForSeconds(cooldown);

        elevatorDoorController?.OpenDoors();

        isBusy = false;
    }
}