using UnityEngine;
using System.Collections;

public class TemporaryMover : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Drag the object you want to move here")]
    public GameObject objectToMove;

    [Tooltip("Time in seconds to wait before returning")]
    public float duration = 1f; 

    [Header("Audio")]
    [Tooltip("Drag your click sound file here")]
    public AudioClip clickSound;

    private bool isMoving = false; // Safety flag to prevent double-clicking
    private AudioSource audioSource;


    private void Start()
    {
        // Get the AudioSource component attached to this object
        audioSource = GetComponent<AudioSource>();
    }

    public void StartMoveSequence()
    {
        if (objectToMove == null)
        {
            Debug.LogError("No object assigned to move!");
            return;
        }

        // Prevent the routine from running if it's already active
        if (isMoving) return;

        if (clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }

        StartCoroutine(MoveRoutine());
    }

    private IEnumerator MoveRoutine()
    {
        isMoving = true;

        // 1. Remember where the object started
        Vector3 originalPosition = objectToMove.transform.position;

        objectToMove.transform.position = originalPosition + new Vector3(0, 0, -0.04f);

        // 3. Wait for the specified duration (15 seconds)
        yield return new WaitForSeconds(duration);

        // 4. Move back to the original position
        objectToMove.transform.position = originalPosition;

        isMoving = false;
    }
}