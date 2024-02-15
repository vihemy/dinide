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
    }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit()
    {
        UIManager.Instance.EnableIdleState(false);
    }
}