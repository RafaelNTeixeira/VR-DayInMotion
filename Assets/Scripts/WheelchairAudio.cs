using UnityEngine;

public class WheelchairAudio : MonoBehaviour
{
    [Header("Audio Setup")]
    [Tooltip("The Child Object with the AudioSource")]
    public AudioSource movementSource; 

    [Header("Movement Settings")]
    public float movementThreshold = 0.1f; // Minimum speed to start sound
    public float maxSpeed = 3.0f;          // The speed where pitch hits maximum
    public float teleportThreshold = 2.0f; // Ignore jumps larger than this

    [Header("Audio Tuning")]
    public float fadeSpeed = 5.0f;         // How fast volume changes
    [Range(0.5f, 1.0f)] public float minPitch = 0.8f; // Deep rumble when slow
    [Range(1.0f, 2.0f)] public float maxPitch = 1.3f; // High whine when fast

    private Vector3 lastPosition;
    private float targetVolume;
    private float targetPitch;

    void Start()
    {
        // Safety Check
        if (movementSource == null)
        {
            Debug.LogError("WheelchairAudio: Drag the 'Movement_Audio' child object here!");
            this.enabled = false;
            return;
        }

        movementSource.loop = true;
        movementSource.volume = 0; 
        movementSource.pitch = minPitch; // Start at low pitch
        if (!movementSource.isPlaying) movementSource.Play();

        lastPosition = transform.position;
    }

    void Update()
    {
        // Calculate Speed (Ignore Height/Y)
        Vector3 currentPosFlat = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 lastPosFlat = new Vector3(lastPosition.x, 0, lastPosition.z);

        float distanceMoved = Vector3.Distance(currentPosFlat, lastPosFlat);
        float currentSpeed = distanceMoved / Time.deltaTime;

        // Teleport Check (Silence immediately)
        if (distanceMoved > teleportThreshold)
        {
            movementSource.volume = 0;
            lastPosition = transform.position;
            return;
        }

        // Wheelchair Logic
        if (currentSpeed > movementThreshold)
        {
            targetVolume = 1.0f; // Moving? Full volume

            // Calculate Pitch based on speed percentage
            // If speed is 0, pitch is min. If speed is maxSpeed, pitch is max.
            float speedPercent = Mathf.Clamp01(currentSpeed / maxSpeed);
            targetPitch = Mathf.Lerp(minPitch, maxPitch, speedPercent);
        }
        else
        {
            targetVolume = 0.0f; // Stopped? Silence
            targetPitch = minPitch; // Reset pitch to low
        }

        // Apply smooth transitions
        // We SmoothDamp the pitch so it doesn't wobble if your hand shakes
        movementSource.volume = Mathf.Lerp(movementSource.volume, targetVolume, Time.deltaTime * fadeSpeed);
        movementSource.pitch = Mathf.Lerp(movementSource.pitch, targetPitch, Time.deltaTime * fadeSpeed);

        lastPosition = transform.position;
    }
}