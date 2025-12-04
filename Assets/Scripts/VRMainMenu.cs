using UnityEngine;
using UnityEngine.SceneManagement;

public class VRMainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name or index of the game scene to load")]
    [SerializeField] private string gameSceneName = "GameScene"; 

    public void OnPlayClicked()
    {
        Debug.Log("Play Button Clicked - Loading Game...");
        SceneManager.LoadScene(gameSceneName);
    }

    public void OnQuitClicked()
    {
        Debug.Log("Quit Button Clicked - Exiting Application.");
        
        Application.Quit();

        // This line makes the Quit button work inside the Unity Editor too
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}