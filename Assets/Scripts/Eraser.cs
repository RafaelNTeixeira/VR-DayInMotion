using UnityEngine;

public class Eraser : MonoBehaviour
{
    [Header("Visual Settings")]
    public Color farColor = Color.yellow; // Bright Yellow
    public Color nearColor = new Color(0.6f, 0.6f, 0f); // Darker/Normal Yellow
    public float pulseSpeed = 3.0f;
    public float stopPulseRange = 2.0f; // Distance to stop pulsing

    [Header("Dialogue Settings")]
    public DialogueController dialogueController;
    [TextArea] public string messageContent = "Thats about all I can clean. Can't reach the top. Let's leave a message to the team now. (Write whatever you want with the markers on the left)";
    [Range(0f, 1f)] public float whitePixelsThreshold = 0.9f; // 0.9 = 90%

    [Header("References")]
    public string playerTag = "Player";
    
    private Renderer toolRenderer;
    private Color originalColor;
    private Transform playerTransform;
    private bool hasTriggeredDialogue = false;
    private float nextCheckTime = 0f;

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
            if (playerObj != null) playerTransform = playerObj.transform;
        }
    }

    void Update()
    {
        if (toolRenderer == null || playerTransform == null) return;

        // PULSE LOGIC
        float distance = Vector3.Distance(transform.position, playerTransform.position);

        if (distance > stopPulseRange)
        {
            // Pulse between Original and Far Color
            float t = Mathf.PingPong(Time.time * pulseSpeed, 1f);
            toolRenderer.material.color = Color.Lerp(originalColor, farColor, t);
        }
        else
        {
            // Reset to normal when close
            toolRenderer.material.color = originalColor;
        }
    }

    void OnDisable()
    {
        // Reset color when script triggers off
        if (toolRenderer != null) toolRenderer.material.color = originalColor;
    }

    void OnCollisionStay(Collision collision)
    {
        // 1. Get Board Reference
        Whiteboard board = collision.gameObject.GetComponent<Whiteboard>();
        if (board == null) return;

        // 2. Erasing Logic
        bool didErase = false;
        foreach (ContactPoint contact in collision.contacts)
        {
            Ray ray = new Ray(contact.point + (contact.normal * 0.1f), -contact.normal);
            RaycastHit hit;

            if (collision.collider.Raycast(ray, out hit, 1.0f))
            {
                board.EraseAt(hit.textureCoord);
                didErase = true;
            }
        }

        // Check Progress (Only if we actually erased something this frame)
        if (didErase && !hasTriggeredDialogue && dialogueController != null)
        {
            // Performance Optimization: Only check pixels every 0.5 seconds
            if (Time.time > nextCheckTime)
            {
                nextCheckTime = Time.time + 0.5f; // Reset timer

                // Check the percentage
                float currentCleanParams = board.GetCleanPercentage();
                
                if (currentCleanParams >= whitePixelsThreshold)
                {
                    // Trigger Dialogue
                    dialogueController.Think(messageContent, 1.0f, 0.8f, 0.2f);
                    hasTriggeredDialogue = true;
                    Debug.Log("Success! Board is " + (currentCleanParams * 100) + "% clean.");
                }
            }
        }
    }
}