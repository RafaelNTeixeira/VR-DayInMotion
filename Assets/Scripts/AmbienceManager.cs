using UnityEngine;
using System.Collections;

// Manager class to handle ambience sound changes
public class AmbienceManager : MonoBehaviour
{
    [Header("Audio Setup")]
    public AudioSource ambienceSource;
    public float fadeDuration = 2.0f; // How long the crossfade takes

    // Singleton pattern: Allows other scripts to find this easily
    public static AmbienceManager instance;

    void Awake()
    {
        // Simple Singleton setup
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // Ensure the AudioSource loops
        if (ambienceSource != null) ambienceSource.loop = true;
    }

    // Call this function from the triggers
    public void ChangeAmbience(AudioClip newClip)
    {
        // If we are already playing this sound, do nothing
        if (ambienceSource.clip == newClip) return;

        StopAllCoroutines(); // Stop any current fading
        StartCoroutine(FadeToNewClip(newClip));
    }

    private IEnumerator FadeToNewClip(AudioClip newClip)
    {
        float startVolume = ambienceSource.volume;

        // Fade OUT current sound
        if (ambienceSource.isPlaying)
        {
            while (ambienceSource.volume > 0)
            {
                ambienceSource.volume -= startVolume * Time.deltaTime / (fadeDuration / 2);
                yield return null;
            }
        }

        // Swap the clip
        ambienceSource.clip = newClip;
        ambienceSource.Play();

        // Fade IN new sound
        float targetVolume = 0.5f;

        while (ambienceSource.volume < targetVolume)
        {
            ambienceSource.volume += Time.deltaTime / (fadeDuration / 2);
            yield return null;
        }
        
        ambienceSource.volume = targetVolume;
    }
}