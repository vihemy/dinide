using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject IdlePanel;
    [SerializeField] private GameObject InputPanel;
    [SerializeField] private GameObject InputPanelLock;
    [SerializeField] private Image nut;
    [SerializeField] private Image checkmark;

    public void EnableIdleState(bool state)
    {
        IdlePanel.SetActive(state);
    }
    public void EnableInputState(bool state)
    {
        InputPanel.SetActive(state);
    }
    public void LockInputPanel(bool state)
    {
        InputPanelLock.SetActive(state);
    }

    public void SetPanelIcon(PanelIcon icon)
    {
        switch (icon)
        {
            case PanelIcon.Nut:
                nut.enabled = true;
                checkmark.enabled = false;
                break;
            case PanelIcon.Checkmark:
                nut.enabled = false;
                checkmark.enabled = true;
                break;
            default:
                break;
        }
    }

    public enum PanelIcon
    {
        Nut,
        Checkmark
    }
}