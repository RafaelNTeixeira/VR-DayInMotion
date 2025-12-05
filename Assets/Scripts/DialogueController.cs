using UnityEngine;
using TMPro;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    [Header("UI References")]
    public Canvas thoughtCanvas;
    public TextMeshProUGUI thoughtText;

    [Header("Audio References")]
    public AudioSource typingAudioSource;

    [Header("Dialogue Input")]
    [TextArea(3, 8)]
    public string startingThought = ""; 
    public bool showOnStart = false; 
    public float displayTime = 3f;

    [Header("Settings")]
    public float distance = 1.5f;
    public float heightOffset = -0.3f;
    public float followSpeed = 5f;

    [Header("Typewriter Settings")]
    [Tooltip("Seconds between each letter appearing")]
    public float typingSpeed = 0.05f; 

    private Transform head;
    private Vector3 targetPos;
    
    private Coroutine hideCoroutine;
    private Coroutine typingCoroutine;

    void Start()
    {
        if (Camera.main != null)
        {
            head = Camera.main.transform;
        }
        else
        {
            Debug.LogError("DialogueController: No Main Camera found!");
            enabled = false;
            return;
        }

        // Auto-fetch audio source if forgotten
        if (typingAudioSource == null) typingAudioSource = GetComponent<AudioSource>();

        thoughtCanvas.enabled = false;

        if (showOnStart && !string.IsNullOrEmpty(startingThought))
        {
            Think(startingThought, displayTime);
        }
    }

    void LateUpdate()
    {
        if (!thoughtCanvas.enabled || head == null) return;

        Vector3 screenForward = head.forward; 
        
        targetPos = head.position + screenForward * distance;
        targetPos += head.up * heightOffset; 
        
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

        Quaternion targetRot = Quaternion.LookRotation(transform.position - head.position, head.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
    }

    public void Think(string thought, float duration)
    {
        thoughtCanvas.enabled = true;
        
        // Stop any existing routines/audio so they don't overlap
        if (hideCoroutine != null) StopCoroutine(hideCoroutine);
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        
        // Force stop audio in case we interrupted the previous sentence mid-type
        if (typingAudioSource != null) typingAudioSource.Stop();

        // Start the typewriter effect
        typingCoroutine = StartCoroutine(TypewriterRoutine(thought, duration));
    }

    private IEnumerator TypewriterRoutine(string textToType, float duration)
    {
        thoughtText.text = textToType;
        thoughtText.ForceMeshUpdate();
        thoughtText.maxVisibleCharacters = 0;

        int totalVisibleCharacters = thoughtText.textInfo.characterCount;
        int counter = 0;

        // AUDIO START
        if (typingAudioSource != null)
        {
            typingAudioSource.Play();
        }

        // Reveal characters one by one
        while (counter <= totalVisibleCharacters)
        {
            thoughtText.maxVisibleCharacters = counter;
            counter++;
            
            yield return new WaitForSeconds(typingSpeed);
        }

        // AUDIO STOP
        if (typingAudioSource != null)
        {
            typingAudioSource.Stop();
        }

        hideCoroutine = StartCoroutine(HideAfterSeconds(duration));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        thoughtCanvas.enabled = false;
    }
}