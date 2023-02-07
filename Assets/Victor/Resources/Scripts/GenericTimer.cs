using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTimer
{
    #region CountDownTimer

    public Action OnTimeIsUpLogic = () => { };
    private bool timerActive = false;
    private bool isContinuous = false;
    public int Iterations { get; private set; } = 0;
    public float CountDownTime { get; private set; }
    public float CountDownTimer { get; private set; }
    
    public GenericTimer(float timeDuration, bool isContinuous)
    {
        SetDuration(timeDuration);
        SetIsContinuous(isContinuous);
    }

    public void SetDuration(float timeDuration)
    {
        CountDownTime = timeDuration;
        CountDownTimer = CountDownTime;
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
            CountDownTimer -= Time.deltaTime;
            if (CountDownTimer <= 0)
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
        {
            StartTimer();
        }
        else
        {
            EndTimer();
        }
    }

    private void EndTimer()
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
