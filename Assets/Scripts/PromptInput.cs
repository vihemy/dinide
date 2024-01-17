using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PromptInput : Singleton<PromptInput>
{
    private string prompt;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private EntryDisplayer entryDisplayer;

    private void Start()
    {
        ResetInputField();
    }

    public void CreateEntryFromPrompt()
    {
        prompt = inputField.text;
        DalleAPIManager.Instance.RequestDalle(prompt);
        Debug.Log(prompt);
        ResetInputField();
    }


    private void ResetInputField()
    {
        inputField.text = "";
        inputField.Select();
    }
}
