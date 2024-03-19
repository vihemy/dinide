using UnityEngine;

public class ActivateAllDisplays : MonoBehaviour
{
    [SerializeField] private Camera[] cameras; // Assign these cameras in the inspector

    void Start()
    {
        Debug.Log("Displays connected: " + Display.displays.Length);
        for (int i = 0; i < Display.displays.Length; i++)
        {
            Display.displays[i].Activate();
        }
    }
}
