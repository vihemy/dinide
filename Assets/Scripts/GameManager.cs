using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private StateManager stateManager = new StateManager();
    public TimerWithCoroutine timer;
    [SerializeField] private int timerDuration = 30;
    public int TimerDuration => timerDuration;
    void Start()
    {
        InstantiateEntrySystem();
        stateManager.ChangeState(new IdleState());
        timer.StartTimer(timerDuration, ResetGameToIdle);
    }

    void Update()
    {
        if (Input.touchCount > 0 || Input.anyKeyDown)
        {
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        if (stateManager.CurrentState is InputState)
        {
            timer.StopTimer();
            timer.StartTimer(timerDuration, ResetGameToIdle);
        }
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

    public void ResetGameToIdle()
    {
        stateManager.ChangeState(new IdleState());
    }
}