using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DashboardManager : Singleton<DashboardManager>
{
    [SerializeField] private TextMeshProUGUI statusText;

    public void IdleDashBoard()
    {
        statusText.text = "Venter på idé";
    }

    public void StartDashBoard()
    {
        statusText.text = "Behandler idé";
    }

    public void StopDashBoard()
    {
        statusText.text = "Behandling færdig";
    }
}