using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DashboardManager : Singleton<DashboardManager>
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Animator animator;

    public void IdleDashBoard()
    {
        statusText.text = "Venter på idé";
    }

    public void StartDashBoard()
    {
        statusText.text = "Behandler idé";
        animator.SetTrigger("Start");
        animator.ResetTrigger("Stop");
    }

    public void StopDashBoard()
    {
        statusText.text = "Behandling færdig";
        animator.SetTrigger("Stop");
        animator.ResetTrigger("Start");
    }
}