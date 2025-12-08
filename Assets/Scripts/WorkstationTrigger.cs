using UnityEngine;

public class WorkstationTrigger : MonoBehaviour
{
    [Tooltip("Drag the sandwich recipe UI panel (Canvas/GameObject) here.")]
    public GameObject sandwichUI;
    
    [Tooltip("Tag on the Player's Rig (e.g., 'Player' or 'XR Rig').")]
    public string playerTag = "Player";

    void Start()
    {
        // Ensure the UI starts hidden
        if (sandwichUI != null)
        {
            sandwichUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered workstation. Enabling UI.");
            if (sandwichUI != null)
            {
                // Enable the UI (e.g., the list of ingredients)
                sandwichUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the object leaving the trigger is the player
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player left workstation. Disabling UI.");
            if (sandwichUI != null)
            {
                // Disable the UI
                sandwichUI.SetActive(false);
            }
        }
    }
}