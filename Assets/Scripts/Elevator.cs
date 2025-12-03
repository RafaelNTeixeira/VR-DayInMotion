using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public float moveAmount = 5f;    // How much the elevator moves
    public float startDelay = 3f;    // Wait before moving
    public float cooldown = 5f;      // Wait after moving before next action

    private bool isBusy = false;     // Prevents multiple triggers
    private bool moveUp = true;      // Alternates direction

    private void OnTriggerEnter(Collider other)
    {
        if (!isBusy && other.CompareTag("Player"))
        {
            StartCoroutine(MoveElevator(other.transform));
        }
    }

    private IEnumerator MoveElevator(Transform player)
    {
        isBusy = true;

        // Wait before moving
        yield return new WaitForSeconds(startDelay);

        // Calculate movement
        Vector3 delta;
        if (moveUp)
        {
            delta = Vector3.up * moveAmount;
        }
        else
        {
            delta = Vector3.down * moveAmount;
        }

        // Move elevator and player manually
        transform.position += delta;
        player.position += delta;

        // Toggle direction for next time
        moveUp = !moveUp;

        // Wait cooldown before allowing next trigger
        yield return new WaitForSeconds(cooldown);

        isBusy = false;
    }
}
