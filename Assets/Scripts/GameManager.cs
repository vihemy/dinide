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
        SlideshowController.Instance.RestartSlideshow(); // slideshow needs to be instantiated before entries are loaded
    }

    public void OnStartGame()
    {
        stateManager.ChangeState(new InputState());
    }

    public void OnSubmitIdea()
    {
        stateManager.ChangeState(new ProcessingState());
        PromptManager.Instance.CreateEntryFromPrompt();
    }

    public void OnFinishProcessing()
    {
        stateManager.ChangeState(new DeliverState());
    }

    public void OnResetGame()
    {
        stateManager.ChangeState(new IdleState());
    }
}