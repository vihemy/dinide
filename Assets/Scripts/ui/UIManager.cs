using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject IdlePanel;
    [SerializeField] private GameObject InputPanel;
    [SerializeField] private GameObject InputPanelLock;

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
}