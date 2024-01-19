using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : Singleton<Logger>
{
    [SerializeField]
    private bool showLogs;
    public void Log(object message)
    {
        if (showLogs)
        {
            Debug.Log(message);
        }
    }
}
