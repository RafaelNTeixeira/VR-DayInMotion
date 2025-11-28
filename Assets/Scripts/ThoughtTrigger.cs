using UnityEngine;

// Class to trigger thoughts when the player enters a collider
public class ThoughtTrigger : MonoBehaviour
{
    public DialogueController dialogueController;
    [TextArea(3, 8)]
    public string thoughtText;
    public float displayTime = 5f;
    public bool wasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !wasTriggered)
        {
            wasTriggered = true;
            dialogueController.Think(thoughtText, displayTime);
        }
    }
}
