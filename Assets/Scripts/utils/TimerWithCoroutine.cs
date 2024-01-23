using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
public class TimerWithCoroutine : MonoBehaviour
{
    /// SCRIPT FROM HERE: https://blog.yarsalabs.com/simple-timer-in-unity-part1/

    private int _timer;
    /// <summary>
    /// Ienumerator which will holds the Ienumerator method that is used to run the timer.
    /// </summary>
    private IEnumerator _timerCoroutine;

    /// <summary>
    /// Triggered when timer is completed and timeout is to be called.
    /// </summary>
    public Action OnTimeOut;

    private void Start()
    {
    }

    private void TimeOut()
    {
        Debug.Log("Your timer is up.");
    }

    public void StartTimer(int timer, Action onTimeOut)
    {
        OnTimeOut = onTimeOut;
        //Reset the timer to 0 before starting the timer
        _timer = 0;
        _timerCoroutine = StartTimer(timer);
        StartCoroutine(_timerCoroutine);
    }

    private IEnumerator StartTimer(int totalTime)
    {
        while (_timer < totalTime)
        {
            //waiting 1 second in real time and increasing the timer value
            yield return new WaitForSecondsRealtime(1);
            _timer++;
        }

        //trigger the timeout action to inform that the time is up.
        OnTimeOut?.Invoke();
    }

    public void StopTimer()
    {
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
            OnTimeOut = null;
        }
    }
}
