using UnityEngine;
using System.Collections.Generic;

public class SandwichBuilder : MonoBehaviour
{
    [System.Serializable]
    public struct SandwichLayer
    {
        public string name; 
        public IngredientType requiredType; 
        public GameObject visualObject; 
    }

    [Header("Setup")]
    public List<SandwichLayer> sandwichSteps; 
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip errorSound;

    private int currentStepIndex = 0;

    void Start()
    {
        // 1. Hide all the sandwich parts at the start
        foreach (var step in sandwichSteps)
        {
            // FIX: Changed 'visualVisual' to 'visualObject'
            if(step.visualObject != null) 
                step.visualObject.SetActive(false);
        }
    }

    // Detecting the loose ingredient
    void OnTriggerEnter(Collider other)
    {
        // 1. Is the sandwich already finished?
        if (currentStepIndex >= sandwichSteps.Count) return;

        // 2. Is the object an ingredient?
        LooseIngredient incoming = other.GetComponent<LooseIngredient>();
        if (incoming != null)
        {
            // 3. Check Order: Is this the SPECIFIC ingredient we need right now?
            IngredientType neededType = sandwichSteps[currentStepIndex].requiredType;

            if (incoming.myType == neededType)
            {
                // CORRECT INGREDIENT!
                AddLayer(incoming.gameObject);
            }
            else
            {
                // WRONG INGREDIENT (e.g. tried to put Avocado before Bacon)
                // Optional: You can remove this 'else' if you want to silently ignore wrong ingredients
                Debug.Log($"Wrong Order! Need {neededType}, got {incoming.myType}");
                if(audioSource && errorSound) audioSource.PlayOneShot(errorSound);
            }
        }
    }

    void AddLayer(GameObject looseObject)
    {
        // 1. Turn on the "Ghost" part in the sandwich
        // FIX: Changed 'visualVisual' to 'visualObject'
        if (sandwichSteps[currentStepIndex].visualObject != null)
        {
            sandwichSteps[currentStepIndex].visualObject.SetActive(true);
        }

        // 2. Play Sound
        if(audioSource && successSound) audioSource.PlayOneShot(successSound);

        // 3. Delete the loose object from the player's hand
        Destroy(looseObject);

        // 4. Advance the step
        currentStepIndex++;

        // 5. Check if finished
        if (currentStepIndex >= sandwichSteps.Count)
        {
            Debug.Log("SANDWICH COMPLETE!");
            // Triggers logic for "Mission Complete" here
        }
    }
}