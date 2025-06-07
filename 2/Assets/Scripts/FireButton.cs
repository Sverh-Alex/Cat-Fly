using UnityEngine;
using UnityEngine.EventSystems;


public class FireButton : MonoBehaviour
{
    private Cat catScript;

    void Start()
    {
        catScript = FindObjectOfType<Cat>();
        if (catScript == null)
        {
            Debug.LogError("Cat script not found in scene!");
        }
    }

    public void OnPointerDown()
    {
        if (catScript != null)
        {
            catScript.fire();
        }
    }
}
