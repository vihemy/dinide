using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingState : IState
{
    public void Enter()
    {
        DashboardManager.Instance.StartDashBoard();
        Logger.Instance.Log("Entering Processing State");
    }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit()
    {
        DashboardManager.Instance.StopDashBoard();
        Logger.Instance.Log("Exiting Processing State");
    }
}