using UnityEngine;

// Class to define ambience zones that change the background sound when the player enters
public class AmbienceZone : MonoBehaviour
{
    [Header("Sound Settings")]
    [Tooltip("The sound to play when Player enters this zone")]
    public AudioClip ambienceSound;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering is the Player
        if (other.CompareTag("Player"))
        {
            // Tell the Manager to switch tracks
            if (AmbienceManager.instance != null)
            {
                AmbienceManager.instance.ChangeAmbience(ambienceSound);
                Debug.Log($"Entered Zone: {gameObject.name}. Playing: {ambienceSound.name}");
            }
        }
    }
}