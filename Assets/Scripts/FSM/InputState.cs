using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : IState
{
    public void Enter()
    {
        UIManager.Instance.EnableInputState(true);
        UIManager.Instance.LockInputPanel(false);
        PromptManager.Instance.ResetInputField();
        GameManager.Instance.timer.StartTimer(GameManager.Instance.TimerDuration, GameManager.Instance.ResetGameToIdle);
        Logger.Instance.Log("Entering Input State");
    }
    public void Execute()
    {

    }
    public void Exit()
    {
        GameManager.Instance.timer.StopTimer();
        Logger.Instance.Log("Exiting Input State");
    }
}