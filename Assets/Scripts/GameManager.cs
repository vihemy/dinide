using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private StateManager stateManager = new StateManager();
    void Start()
    {
        InstantiateEntrySystem();
        stateManager.ChangeState(new IdleState());
    }

    void InstantiateEntrySystem()
    {
        EntryLoader.Instance.LoadLatestEntries();
        SlideshowController.Instance.StartSlideshow(); // slideshow needs to be instantiated before entries are loaded
    }

    public void ResetGame()
    {
        stateManager.ChangeState(new IdleState());
    }

    public void StartGame()
    {
        stateManager.ChangeState(new InputState());
    }

    public void SubmitIdea()
    {
        stateManager.ChangeState(new ProcessingState());
        PromptManager.Instance.CreateEntryFromPrompt();
    }

    public void AbortProcessing(ErrorType errorType)
    {
        stateManager.ChangeState(new InputState());
        AudioManager.Instance.StopAllSounds();
        DashboardManager.Instance.StartIdleAnimation();
        PopupController.Instance.DisplayPopup(errorType);
    }

    public void FinishProcessing()
    {
        stateManager.ChangeState(new OutputState());
    }
}