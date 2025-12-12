using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name or index of the game scene to load")]
    [SerializeField] private string gameSceneName = "GameScene"; 

    [Tooltip("The sound clip to play when a button is clicked.")]
    [SerializeField] private AudioClip clickSound;

    public void OnPlayClicked()
    {
        Debug.Log("Play Button Clicked - Loading Game...");
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }

        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quit Button Clicked - Exiting Application.");

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }
        
        Application.Quit();

        // This line makes the Quit button work inside the Unity Editor too
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}