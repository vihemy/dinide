using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : IState
{
    public void Enter()
    {
        UIManager.Instance.EnableInputState(true);
        UIManager.Instance.LockInputPanel(false);
        Logger.Instance.Log("Entering Input State");
    }
    public void Execute()
    {

    }
    public void Exit()
    {

        Logger.Instance.Log("Exiting Input State");
    }
}