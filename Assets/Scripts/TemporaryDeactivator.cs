using UnityEngine;
using System.Collections;

public class TemporaryDeactivator : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Drag the object you want to disable here")]
    public GameObject objectToDeactivate;

    [Tooltip("Time in seconds to stay deactivated")]
    public float duration = 60f; 

    // Link this method to your Button's OnClick event
    public void DeactivateObject()
    {
        Debug.Log("DeactivateObject method called.");
        if (objectToDeactivate != null)
        {
            StartCoroutine(DisableRoutine());
        }
        else
        {
            Debug.LogError("No object assigned to deactivate!");
        }
    }

    private IEnumerator DisableRoutine()
    {
        // 1. Deactivate the object
        objectToDeactivate.SetActive(true);

        // 2. Wait for the specified duration (60 seconds)
        yield return new WaitForSeconds(duration);

        // 3. Reactivate the object
        objectToDeactivate.SetActive(false);
    }
}