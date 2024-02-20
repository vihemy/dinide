using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DashboardManager : Singleton<DashboardManager>
{
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Animator animator1;
    [SerializeField] private Animator animator2;

    public void IdleDashBoard()
    {
        statusText.text = "Venter på idé";
    }

    public void StartDashBoard()
    {
        statusText.text = "Behandler idé";
        animator1.SetTrigger("Start");
        animator1.ResetTrigger("Stop");

        animator2.SetTrigger("Start");
        animator2.ResetTrigger("Stop");
    }

    public void StopDashBoard()
    {
        statusText.text = "Behandling færdig";
        animator1.SetTrigger("Stop");
        animator1.ResetTrigger("Start");

        animator2.SetTrigger("Stop");
        animator2.ResetTrigger("Start");
    }
}