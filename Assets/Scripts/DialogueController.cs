using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [Header("UI References")]
    public Canvas thoughtCanvas;
    public TextMeshProUGUI thoughtText;

    [Header("Dialogue Input")]
    [TextArea(3, 8)]
    public string startingThought = ""; 
    public bool showOnStart = false; 

    [Header("Settings")]
    public float distance = 1.5f;
    public float heightOffset = -0.3f;
    public float followSpeed = 5f;
    public float appearSpeed = 10f;

    private Transform head;
    private Vector3 targetPos;

    void Start()
    {
        head = Camera.main.transform;
        thoughtCanvas.enabled = false;

        // If you want it to appear automatically
        if (showOnStart && !string.IsNullOrEmpty(startingThought))
        {
            Think(startingThought);
        }
    }

    void LateUpdate()
    {
        if (!thoughtCanvas.enabled) return;

        Vector3 forward = head.forward;
        forward.y = 0;

        targetPos = head.position + forward.normalized * distance;
        targetPos += Vector3.up * heightOffset;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        Quaternion targetRot = Quaternion.LookRotation(transform.position - head.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
    }

    public void Think(string thought)
    {
        thoughtText.text = thought;
        thoughtCanvas.enabled = true;
    }

    public void ClearThought()
    {
        thoughtCanvas.enabled = false;
    }
}
