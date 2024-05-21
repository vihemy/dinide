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
        animator.SetTrigger("Start");
        animator.ResetTrigger("Stop");
    }

    public void StartIdleAnimation()
    {
        animator.SetTrigger("Stop");
        animator.ResetTrigger("Start");
    }

    public void TriggerAnimationEndEvent() // is triggered from RunDashboard animation
    {
        OnDashboardAnimationEnd?.Invoke();
    }
}