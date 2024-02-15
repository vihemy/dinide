using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingState : IState
{
    public void Enter()
    {
        DashboardManager.Instance.StartDashBoard();
    }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit()
    {
        DashboardManager.Instance.StopDashBoard();
    }
}