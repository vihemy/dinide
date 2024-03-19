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
    [SerializeField] private GameObject nutGO;
    [SerializeField] private GameObject checkmarkGO;

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
                nutGO.SetActive(true);
                checkmarkGO.SetActive(false);
                break;
            case PanelIcon.Checkmark:
                nutGO.SetActive(false);
                checkmarkGO.SetActive(true);
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