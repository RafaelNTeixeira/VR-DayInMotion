using UnityEngine;

public class ActivateBoardTask : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Eraser GameObject here that holds the Eraser script.")]
    public Eraser eraserScript;

    [Tooltip("Drag the arrow to activate here.")]
    public GameObject arrowToActivate;
    
    [Tooltip("Tag on the Player's Rig")]
    public string playerTag = "Player";

    void Start()
    {
        // Start with the eraser logic disabled (no pulsing, no erasing)
        if (eraserScript != null)
        {
            eraserScript.enabled = false;
        }

        if (arrowToActivate != null)
        {
            arrowToActivate.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("Player entered. Activating Eraser script.");
            
            // Activate the Eraser script so it starts calculating distance/pulsing
            if (eraserScript != null)
            {
                eraserScript.enabled = true;
            }

            // Activate the arrow
            if (arrowToActivate != null)
            {
                arrowToActivate.SetActive(true);
            }
        }
    }
}