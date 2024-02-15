using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputState : IState
{
    public void Enter()
    {
        UIManager.Instance.EnableInputState(true);
        UIManager.Instance.LockInputPanel(false);
    }
    public void Execute()
    {

    }
    public void Exit()
    {
        UIManager.Instance.LockInputPanel(true);
    }
}