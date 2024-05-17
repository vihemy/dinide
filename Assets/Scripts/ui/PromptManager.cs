using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UltimateClean;

public class PromptManager : Singleton<PromptManager>
{
    [SerializeField] private TMP_InputField promptInputField;
    [SerializeField] private TMP_InputField authorInputField;
    [SerializeField] private TMP_InputField ageInputField;
    [SerializeField] private DalleAPICaller dalleAPICaller;
    [SerializeField] private CompletionAPICaller completionAPICaller;
    public ButtonListener sendButton;

    void Start()
    {
        ResetInputField();
        promptInputField.onEndEdit.AddListener(delegate { ValidateInputs(); });
        authorInputField.onEndEdit.AddListener(delegate { ValidateInputs(); });
        ageInputField.onValueChanged.AddListener(delegate { ValidateInputs(); });
    }

    private void ValidateInputs()
    {
        sendButton.MakeInteractable(!AreFieldsEmptyOrProfane());
    }

    public void ResetInputField()
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
            completionAPICaller.CheckPromptRelated(entry, OnCompletionResponse);
            ResetInputField();
        }
    }

    private EntryData CreateEntryData()
    {
        string prompt = promptInputField.text;
        string author = authorInputField.text;
        string age = ageInputField.text;

        EntryData entry = new EntryData(prompt, author, age); // Be careful to fill out the appropriate fields in the EntryData constructor!

        Logger.Instance.Log($"Input entered: Prompt = {entry.prompt}");
        return entry;
    }

    private void OnCompletionResponse(EntryData entry)
    {
        // After checking relevance, pass the updated entry to DalleAPICaller
        dalleAPICaller.RequestDalle(entry);
    }

    public bool AreFieldsEmptyOrProfane()
    {
        if (AreFieldsEmpty())
        {
            return true;
        }
        else if (DoesFieldsContainProfanity())
        {
            PopupController.Instance.DisplayPopup(ErrorType.Profanity);
            return true;
        }
        return false;
    }

    private bool AreFieldsEmpty()
    {
        return string.IsNullOrEmpty(promptInputField.text) || string.IsNullOrEmpty(authorInputField.text) || string.IsNullOrEmpty(ageInputField.text);
    }

    private bool DoesFieldsContainProfanity()
    {
        return ProfanityFilter.Instance.ContainsProfanity(promptInputField.text) || ProfanityFilter.Instance.ContainsProfanity(authorInputField.text) || ProfanityFilter.Instance.ContainsProfanity(ageInputField.text);
    }

    void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks
        if (promptInputField != null) promptInputField.onValueChanged.RemoveAllListeners();
        if (authorInputField != null) authorInputField.onValueChanged.RemoveAllListeners();
        if (ageInputField != null) ageInputField.onValueChanged.RemoveAllListeners();
    }
}
