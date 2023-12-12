using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PromptInput : MonoBehaviour
{
    private string prompt;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private EntryDisplayer entryDisplayer;

    private void Awake()
    {
        SelectInputField();
    }

    public void CreateEntryFromPrompt()
    {
        prompt = inputField.text;
        Debug.Log(prompt);
        DalleAPIManager.Instance.RequestDalle(prompt);

        // NewEntry();
        ClearInputField();
        SelectInputField();
    }

    // public void NewEntry()
    // {
    //     string prompt = inputField.text;
    //     entryDisplayer.CreateEntry(prompt);
    // }

    private void ClearInputField()
    {
        inputField.text = "";
    }

    private void SelectInputField()
    {
        inputField.Select();
    }
}
