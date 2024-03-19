using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class DashboardManager : Singleton<DashboardManager>
{
    [SerializeField] private Animator animator;

    public Action OnDashboardAnimationEnd;

    void Start()
    {
        animator.ResetTrigger("Start");
        animator.ResetTrigger("Stop");
    }

    public void StartProcessingAnimation()
    {
        animator.ResetTrigger("Start");
        animator.SetTrigger("Start");

    }

    public void StartIdleAnimation()
    {
        animator.ResetTrigger("Stop");
        animator.SetTrigger("Stop");
    }

    public void TriggerAnimationEndEvent() // is triggered from RunDashboard animation
    {
        Debug.Log("DashboardManager.OnDashboardAnimationEnd called");
        OnDashboardAnimationEnd?.Invoke();
    }
}