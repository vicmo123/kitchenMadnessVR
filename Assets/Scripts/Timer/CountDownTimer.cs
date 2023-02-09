using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Default count down timer.
/// <param>timeDuration</param> the first parameter is for setting the duration of the timer.
/// <param>isContinuous</param> the second parameter is used to make the timer loop
/// Add functions to perform when time is up in this maner : timer.OnTimeIsUpLogic += () => { OnTimeIsUpLogic(); };
/// </summary>
public class CountDownTimer
{
    #region CountDownTimer
    public Action OnTimeIsUpLogic = () => { };
    private bool timerActive = false;
    private bool isContinuous = false;
    public int Iterations { get; private set; } = 0;
    public float CountDownTime { get; private set; }
    public float Timer { get; private set; }

    public CountDownTimer(float timeDuration, bool isContinuous)
    {
        SetDuration(timeDuration);
        SetIsContinuous(isContinuous);
    }

    public void SetDuration(float timeDuration)
    {
        CountDownTime = timeDuration;
        Timer = CountDownTime;
    }

    public void StartTimer()
    {
        SetDuration(CountDownTime);
        timerActive = true;
    }

    public void UpdateTimer()
    {
        if (timerActive)
        {
            Timer -= Time.deltaTime;
            if (Timer <= 0)
            {
                InvokeTimer();
                SetDuration(CountDownTime);
            }
        }
    }

    private void InvokeTimer()
    {
        OnTimeIsUpLogic.Invoke();
        Iterations++;

        if (isContinuous)
            StartTimer();
        else
            EndTimer();
    }

    public void EndTimer()
    {
        timerActive = false;
    }

    public void SetIsContinuous(bool value)
    {
        isContinuous = value;
    }

    public void ResetIterations()
    {
        Iterations = 0;
    }
    #endregion
}
