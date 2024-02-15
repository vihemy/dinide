using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class PromptManager : Singleton<PromptManager>
{
    [SerializeField] private TMP_InputField promptInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField ageInputField;

    void Start()
    {
        ResetInputField();
    }

    private void ResetInputField()
    {
        promptInputField.text = "";
        authorInputField.text = "";
        ageInputField.text = "";
        promptInputField.Select();
    }

    public void CreateEntryFromPrompt()
    {
        if (!AreFieldsEmptyOrProfane())
        {
            EntryData entry = CreateEntryData();
            DalleAPICaller.Instance.RequestDalle(entry);
            Debug.Log(entry.prompt + " " + entry.author + " " + entry.age);
            ResetInputField();
        }
    }

    private EntryData CreateEntryData()
    {
        string prompt = promptInputField.text;
        string author = authorInputField.text;
        string age = ageInputField.text;

        EntryData entry = new EntryData(prompt, author, age); // OBS! Be carefull to fill out the appropriate fields in the EntryData constructor!
        return entry;
    }

    private bool AreFieldsEmptyOrProfane()
    {
        if (IsFieldEmpty())
        {
            Debug.Log("Please fill out all fields");
            return true;
        }
        else if (ContainsProfanity(promptInputField.text))
        {
            Debug.Log("Prompt input field contains profanity");
            return true;
        }
        else if (ContainsProfanity(authorInputField.text))
        {
            Debug.Log("Author input field contains profanity");
            return true;
        }
        else if (ContainsProfanity(ageInputField.text))
        {
            Debug.Log("Age input field contains profanity");
            return true;
        }
        return false;
    }

    private bool IsFieldEmpty()
    {
        return string.IsNullOrEmpty(promptInputField.text) || string.IsNullOrEmpty(authorInputField.text) || string.IsNullOrEmpty(ageInputField.text);
    }

    private bool ContainsProfanity(string input)
    {
        return ProfanityFilter.Instance.ContainsProfanity(input);
    }

}