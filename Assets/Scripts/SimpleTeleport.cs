using UnityEngine;

public class SimpleTeleport : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Drag the 'Teleport_Destination' empty object here")]
    [SerializeField] private Transform destination;

    [Tooltip("Should the player rotate to match the destination?")]
    [SerializeField] private bool matchRotation = false;

    // We use OnTriggerEnter to detect when the player walks into the zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the object is the Player
        if (other.CompareTag("Player"))
        {
            TeleportPlayer(other.gameObject);
        }
    }

    private void TeleportPlayer(GameObject player)
    {
        // Find the absolute root of the player
        Transform playerRoot = player.transform.root;

        // If the player has a CharacterController, we must disable it briefly or it will fight the teleport and snap the player back.
        CharacterController cc = playerRoot.GetComponentInChildren<CharacterController>();
        if (cc != null) cc.enabled = false;

        // Move the player to the destination position
        playerRoot.position = destination.position;

        // Rotate the player to match destination rotation if needed
        if (matchRotation)
        {
            playerRoot.rotation = destination.rotation;
        }

        // 6. Turn CharacterController back on
        if (cc != null) cc.enabled = true;

        Debug.Log("Teleported to " + destination.name);
    }
}