using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    public void Enter()
    {
        UIManager.Instance.EnableInputState(false);
        UIManager.Instance.EnableIdleState(true);
        DashboardManager.Instance.IdleDashBoard();
        Logger.Instance.Log("Entering Idle State");
    }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit()
    {
        UIManager.Instance.EnableIdleState(false);
        Logger.Instance.Log("Exiting Idle State");
    }
}