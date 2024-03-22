using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessingState : IState
{
    public void Enter()
    {
        DashboardManager.Instance.StartProcessingAnimation();
        UIManager.Instance.LockInputPanel(true);
        UIManager.Instance.SetPanelIcon(UIManager.PanelIcon.ProgressBar);
        Logger.Instance.Log("Entering Processing State");
    }
    public void Execute() { /* Logic to update during the idle state */ }

    public void Exit()
    {
        DashboardManager.Instance.StartIdleAnimation();
        AudioManager.Instance.StopAllSounds();
        Logger.Instance.Log("Exiting Processing State");
    }
}