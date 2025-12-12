using UnityEngine;

public class ActivateIndicatorArrow : MonoBehaviour
{
    [Tooltip("Drag the arrow to activate here.")]
    public GameObject arrowToActivate;
    
    [Tooltip("Tag on the Player's Rig")]
    public string playerTag = "Player";

    void Start()
    {
        // Ensure the arrow to activate starts hidden
        if (arrowToActivate != null)
        {
            arrowToActivate.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered arrow. Enabling next arrow.");
            if (arrowToActivate != null)
            {
                // Enable the arrow
                arrowToActivate.SetActive(true);
            }
        }
    }
}