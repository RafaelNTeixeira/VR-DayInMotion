using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VRMainMenu : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("The name or index of the game scene to load")]
    [SerializeField] private string gameSceneName = "GameScene"; 

    [Tooltip("The sound clip to play when a button is clicked.")]
    [SerializeField] private AudioClip clickSound;

    public Slider speedSlider;
    public Slider rotationSpeedSlider;
    public Slider accelerationSlider;
    public GameObject startButton;
    public GameObject quitButton;
    public GameObject settingsButton;
    public GameObject backButton;

    public void OnPlayClicked()
    {
        Debug.Log("Play Button Clicked - Loading Game...");
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }

        SceneManager.LoadScene(gameSceneName);
    }

    private void Start()
    {
        // Initialize slider with default value
        if (GameSettings.Instance != null)
        {
            speedSlider.value = GameSettings.Instance.chosenForwardSpeed;
            rotationSpeedSlider.value = GameSettings.Instance.chosenTurnSpeed;
            accelerationSlider.value = GameSettings.Instance.chosenAcceleration;
            
            // Listen for changes
            speedSlider.onValueChanged.AddListener(OnSpeedChanged);
            rotationSpeedSlider.onValueChanged.AddListener(OnRotationSpeedChanged);
            accelerationSlider.onValueChanged.AddListener(OnAccelerationChanged);
        }
    }

    public void OnSpeedChanged(float val)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetSpeed(val);
        }
    }

    public void OnRotationSpeedChanged(float val)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetRotation(val);
        }
    }

    public void OnAccelerationChanged(float val)
    {
        if (GameSettings.Instance != null)
        {
            GameSettings.Instance.SetAcceleration(val);
        }
    }

    public void OnSettingsClicked()
    {
        Debug.Log("Settings Button Clicked.");

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }
        
        startButton.SetActive(false);
        quitButton.SetActive(false);
        settingsButton.SetActive(false);
        speedSlider.gameObject.SetActive(true);
        rotationSpeedSlider.gameObject.SetActive(true);
        accelerationSlider.gameObject.SetActive(true);
        backButton.SetActive(true);
    }

    public void OnBackClicked()
    {
        Debug.Log("Back Button Clicked.");

        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, transform.position);
        }
        
        startButton.SetActive(true);
        quitButton.SetActive(true);
        settingsButton.SetActive(true);
        speedSlider.gameObject.SetActive(false);
        rotationSpeedSlider.gameObject.SetActive(false);
        accelerationSlider.gameObject.SetActive(false);
        backButton.SetActive(false);
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