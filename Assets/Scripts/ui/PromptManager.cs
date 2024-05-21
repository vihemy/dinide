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
    [SerializeField] private ButtonListener sendButton;

    void Start()
    {
        ResetInputField();
        AddEndEditListeners();
    }

    private void AddEndEditListeners()
    {
        promptInputField.onEndEdit.AddListener(OnFieldEndEdit);
        authorInputField.onEndEdit.AddListener(OnFieldEndEdit);
        ageInputField.onValueChanged.AddListener(OnFieldEndEdit);
    }

    private void OnFieldEndEdit(string input)
    {
        if (ProfanityFilter.Instance.ContainsProfanity(input))
        {
            PopupController.Instance.DisplayPopup(ErrorType.Profanity);
        }

        ValidateInputs();
    }

    private void ValidateInputs()
    {
        sendButton.MakeInteractable(AreAllFieldsValid());
    }

    private bool AreAllFieldsValid()
    {
        return !AreFieldsEmpty() && !DoesAnyFieldContainProfanity();
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
        if (AreAllFieldsValid())
        {
            EntryData entry = CreateEntryData();
            completionAPICaller.CheckPromptRelated(entry, OnCompletionResponse);
            ResetInputField();
        }
    }

    private EntryData CreateEntryData()
    {
        return new EntryData(promptInputField.text, authorInputField.text, ageInputField.text);
    }

    private void OnCompletionResponse(EntryData entry)
    {
        dalleAPICaller.RequestDalle(entry);
    }

    private bool AreFieldsEmpty()
    {
        return string.IsNullOrEmpty(promptInputField.text) ||
               string.IsNullOrEmpty(authorInputField.text) ||
               string.IsNullOrEmpty(ageInputField.text);
    }

    private bool DoesAnyFieldContainProfanity()
    {
        return ProfanityFilter.Instance.ContainsProfanity(promptInputField.text) ||
               ProfanityFilter.Instance.ContainsProfanity(authorInputField.text) ||
               ProfanityFilter.Instance.ContainsProfanity(ageInputField.text);
    }

    void OnDestroy()
    {
        RemoveEndEditListeners();
    }

    private void RemoveEndEditListeners()
    {
        if (promptInputField != null) promptInputField.onEndEdit.RemoveAllListeners();
        if (authorInputField != null) authorInputField.onEndEdit.RemoveAllListeners();
        if (ageInputField != null) ageInputField.onEndEdit.RemoveAllListeners();
    }
}
