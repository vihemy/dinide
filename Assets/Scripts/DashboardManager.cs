using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DashboardManager : Singleton<DashboardManager>
{
    public void StartDashBoard()
    {
        Debug.Log("Starting Dashboard");
    }

    public void StopDashBoard()
    {
        Debug.Log("Stopping Dashboard");
    }
}