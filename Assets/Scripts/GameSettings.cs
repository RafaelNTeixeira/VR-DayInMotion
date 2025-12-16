using UnityEngine;

public class GameSettings : MonoBehaviour
{
    // Singleton Instance
    public static GameSettings Instance;

    [Header("Global Settings")]
    public float chosenForwardSpeed = 3f;
    public float chosenTurnSpeed = 90f;
    public float chosenAcceleration = 5f;

    private void Awake()
    {
        // Check if an instance already exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
            return;
        }

        // Set the instance and make it persistent
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Call this from your UI Slider (0 to 1, or raw value)
    public void SetSpeed(float speed)
    {
        chosenForwardSpeed = speed;
    }

    // Call this from your UI Slider (60 to 180, or raw value)
    public void SetRotation(float rotationSpeed)
    {
        chosenTurnSpeed = rotationSpeed;
    }

    public void SetAcceleration(float acceleration)
    {
        chosenAcceleration = acceleration;
    }
}