using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class PromptInput : Singleton<PromptInput>
{
    [SerializeField] private TMP_InputField promptInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField ageInputField;
    [SerializeField] private EntryDisplayer entryDisplayer;

    private void Start()
    {
        ResetInputField();
    }

    public void CreateEntryFromPrompt()
    {
        if (CheckForEmptyFields())
        {
            Debug.Log("Please fill out all fields");
        }
        else
        {
            EntryData entry = CreateEntryData();
            DalleAPIManager.Instance.RequestDalle(entry);
            Debug.Log(entry.prompt + " " + entry.author + " " + entry.age);
            ResetInputField();
        }
    }

    private EntryData CreateEntryData()
    {
        string prompt = promptInputField.text;
        string author = authorInputField.text;
        int age = Convert.ToInt32(ageInputField.text);

        EntryData entry = new EntryData(prompt, author, age);
        return entry;
    }

    private bool CheckForEmptyFields()
    {
        return string.IsNullOrEmpty(promptInputField.text) || string.IsNullOrEmpty(authorInputField.text) || string.IsNullOrEmpty(ageInputField.text);
    }

    private void ResetInputField()
    {
        promptInputField.text = "";
        authorInputField.text = "";
        ageInputField.text = "";
        promptInputField.Select();
    }
}
