using UnityEngine;
using TMPro;
using System.Collections;

// Class to manage displaying dialogue/thoughts
public class DialogueController : MonoBehaviour
{
    [Header("UI References")]
    public Canvas thoughtCanvas;
    public TextMeshProUGUI thoughtText;

    [Header("Dialogue Input")]
    [TextArea(3, 8)]
    public string startingThought = ""; 
    public bool showOnStart = false; 
    public float displayTime = 3f;

    [Header("Settings")]
    public float distance = 1.5f;
    public float heightOffset = -0.3f;
    public float followSpeed = 5f;
    public float appearSpeed = 10f;

    private Transform head;
    private Vector3 targetPos;
    private Coroutine hideCoroutine;

    void Start()
    {
        head = Camera.main.transform;
        thoughtCanvas.enabled = false;

        // If you want it to appear automatically
        if (showOnStart && !string.IsNullOrEmpty(startingThought))
        {
            Think(startingThought, displayTime);
        }
    }

    void LateUpdate()
    {
        if (!thoughtCanvas.enabled) return;

        Vector3 screenForward = head.forward; 
        
        // Calculate base position in front of camera
        targetPos = head.position + screenForward * distance;
        targetPos += head.up * heightOffset; 
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        Quaternion targetRot = Quaternion.LookRotation(transform.position - head.position, head.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
    }

    // Method to display a thought for a certain duration
    public void Think(string thought, float duration)
    {
        thoughtText.text = thought;
        thoughtCanvas.enabled = true;

        // Stop any existing hide coroutine
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        // Start new coroutine to hide after duration
        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    // Method to hide the thought after a delay
    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        thoughtCanvas.enabled = false;
    }
}
