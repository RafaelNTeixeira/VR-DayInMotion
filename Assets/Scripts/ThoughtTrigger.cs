using UnityEngine;
using UnityEngine.SceneManagement;

public class ThoughtTrigger : MonoBehaviour
{
    public DialogueController dialogueController;
    
    [TextArea(3, 8)]
    public string thoughtText;
    public float displayTime = 5f;
    
    [Header("Final Settings")]
    public bool isFinalDialogue = false;
    public Animator closingEyesAnimator;
    public string closingAnimationName = "CloseEyes"; 
    public string menuSceneName = "MainMenu";
    public float animationDuration = 2.0f; 

    private bool wasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wasTriggered)
        {
            wasTriggered = true;
            
            // Show the text
            dialogueController.Think(thoughtText, displayTime);

            // If this is the end, Schedule the next steps
            if (isFinalDialogue)
            {
                // "Call the function 'PlayAnimation' after 'displayTime' seconds"
                Invoke(nameof(PlayAnimation), displayTime + 3.0f);
            }
        }
    }

    // This runs automatically after the text finishes
    private void PlayAnimation()
    {
        if (closingEyesAnimator != null)
        {
            closingEyesAnimator.Play(closingAnimationName);
        }

        // "Call the function 'LoadMenu' after 'animationDuration' seconds"
        Invoke(nameof(LoadMenu), animationDuration);
    }

    // This runs automatically after the animation finishes
    private void LoadMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}