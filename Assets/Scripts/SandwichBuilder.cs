using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SandwichBuilder : MonoBehaviour
{
    [System.Serializable]
    public struct SandwichLayer
    {
        public string name; 
        public IngredientType requiredType; 
        public GameObject visualObject; 
    }

    public DialogueController dialogueController;

    [Header("Setup")]
    public List<SandwichLayer> sandwichSteps; 
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip errorSound;

    [Header("After Completion")]
    public GameObject doorIndicatorArrow;
    public GameObject doorDialogueTrigger;
    public XRGrabInteractable doorOpener;

    private int currentStepIndex = 0;

    void Start()
    {
        // Hide all the sandwich parts at the start
        foreach (var step in sandwichSteps)
        {
            if (step.visualObject != null) 
                step.visualObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Is the sandwich already finished?
        if (currentStepIndex >= sandwichSteps.Count) return;

        // Is the object an ingredient?
        LooseIngredient incoming = other.GetComponent<LooseIngredient>();
        if (incoming != null)
        {
            // Check Order: Is this the SPECIFIC ingredient we need right now?
            IngredientType neededType = sandwichSteps[currentStepIndex].requiredType;

            if (incoming.myType == neededType)
            {
                // CORRECT INGREDIENT!
                AddLayer(incoming.gameObject);
            }
            else
            {
                // WRONG INGREDIENT (e.g. tried to put Avocado before Bacon)
                Debug.Log($"Wrong Order! Need {neededType}, got {incoming.myType}");
                if(audioSource && errorSound) audioSource.PlayOneShot(errorSound);
            }
        }
    }

    void AddLayer(GameObject looseObject)
    {
        // Turn on the "Ghost" part in the sandwich
        if (sandwichSteps[currentStepIndex].visualObject != null)
        {
            sandwichSteps[currentStepIndex].visualObject.SetActive(true);
        }

        // Play Sound
        if(audioSource && successSound) audioSource.PlayOneShot(successSound);

        // Delete the loose object from the player's hand
        Destroy(looseObject);

        // Advance the step
        currentStepIndex++;

        // Check if finished
        if (currentStepIndex == sandwichSteps.Count - 2)
        {
            //Activate thought bubble for forgetting ketchup
            dialogueController.Think("Can't believe I forgot to place the ketchup bottle on the counter! It should be on the shelf to my right.", 7f);
        }

        if (currentStepIndex >= sandwichSteps.Count)
        {
            Debug.Log("SANDWICH COMPLETE!");

            // Enable arrow indicator to door
            if (doorIndicatorArrow != null)
                doorIndicatorArrow.SetActive(true);

            // Enable door dialogue
            if (doorDialogueTrigger != null)
                doorDialogueTrigger.SetActive(true);

            // Enable script to open door
            if (doorOpener != null)
                doorOpener.enabled = true;
        }
    }
}