using UnityEngine;

public class Eraser : MonoBehaviour
{
    [Header("Visual Settings")]
    public Color farColor = Color.yellow; // Color when out of range
    public Color nearColor = new Color(1, 0.92f, 0.016f, 0.5f); // Soft yellow
    public float pulseSpeed = 3.0f;
    public float stopPulseRange = 2.0f; // Distance to stop pulsing (e.g. when player is close to pick it up)

    [Header("References")]
    public string playerTag = "Player";
    
    // Internal variables
    private Renderer toolRenderer;
    private Color originalColor;
    private Transform playerTransform;

    void OnEnable()
    {
        // Setup Renderer
        toolRenderer = GetComponent<Renderer>();
        if (toolRenderer != null)
        {
            originalColor = toolRenderer.material.color;
        }

        // Find Player
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (toolRenderer == null || playerTransform == null) return;

        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stopPulseRange)
        {
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            toolRenderer.material.color = Color.Lerp(originalColor, farColor, t);
        }
        else
        {
            toolRenderer.material.color = originalColor;
        }
    }

    void OnDisable()
    {
        // Reset color when script is turned off
        if (toolRenderer != null)
        {
            toolRenderer.material.color = originalColor;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        // 1. Check if we hit the whiteboard script
        Whiteboard board = collision.gameObject.GetComponent<Whiteboard>();
        if (board == null) return;

        // 2. Loop through touch points
        foreach (ContactPoint contact in collision.contacts)
        {
            Ray ray = new Ray(contact.point + (contact.normal * 0.1f), -contact.normal);
            RaycastHit hit;

            if (collision.collider.Raycast(ray, out hit, 1.0f))
            {
                board.EraseAt(hit.textureCoord);
            }
        }
    }
}