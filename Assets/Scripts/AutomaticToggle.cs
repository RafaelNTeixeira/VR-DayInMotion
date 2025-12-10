using UnityEngine;

public class AutomaticToggle : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    // Runs immediately when this object is activated
    void OnEnable()
    {
        if (object1 != null)
            object1.SetActive(true);
            object2.SetActive(false);
    }

    // Runs immediately when this object is deactivated
    void OnDisable()
    {
        if (object1 != null)
            object1.SetActive(false);
            object2.SetActive(true);
    }
}