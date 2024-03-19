using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputState : IState
{
    public void Enter()
    {
        Logger.Instance.Log("Entering Output State");
        UIManager.Instance.SetPanelIcon(UIManager.PanelIcon.Checkmark);
    }
    public void Execute() { /* Logic to update during the idle state */ }
    public void Exit()
    {
        UIManager.Instance.LockInputPanel(false);
        Logger.Instance.Log("Exiting Output State");
    }
}