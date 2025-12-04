using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    [Header("Animation Settings")]
    [Tooltip("How fast the arrow bobbles up and down.")]
    [SerializeField] private float speed = 2.0f;

    [Tooltip("How far it travels from its starting position.")]
    [SerializeField] private float height = 0.5f;

    [Header("Interaction")]
    private string playerTag = "Player";

    [Tooltip("The sound clip to play when the arrow is collected.")]
    [SerializeField] private AudioClip collectSound;

    [Tooltip("Volume of the sound (0 to 1).")]
    [Range(0f, 1f)]
    [SerializeField] private float soundVolume = 1.0f;

    private Vector3 startPosition;

    void Start()
    {
        // Record the starting position so we bobble around it
        startPosition = transform.position;
    }

    void Update()
    {
        // Smooth Up and Down animation using a Sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the thing that touched the arrow is the Player
        if (other.CompareTag(playerTag))
        {
            if (collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position, soundVolume);
            }
            // Destroy this game object
            Destroy(gameObject);
        }
    }
}