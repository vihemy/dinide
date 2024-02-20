using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UltimateClean;
public class ButtonListener : MonoBehaviour
{
    private CleanButton button => GetComponent<CleanButton>();

    void Start()
    { // used instead of inspector On Click event for better searchability in vs code
        button.onClick.AddListener(() =>
        {
            OnButtonPress();
        });

        MakeInteractable(false);
    }

    public void OnButtonPress()
    {
        GameManager.Instance.SubmitIdea();
    }

    public void MakeInteractable(bool state)
    {
        button.interactable = state;
    }
}
