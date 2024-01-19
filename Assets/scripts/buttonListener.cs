using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    private Button button => GetComponent<Button>();
    // Start is called before the first frame update
    void Start()
    { // used instead of inspector On Click event for better searchability in vs code
        button.onClick.AddListener(() =>
        {
            PromptInput.Instance.CreateEntryFromPrompt();
        });
    }
}
