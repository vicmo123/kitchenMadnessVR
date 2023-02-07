using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTimer
{
    #region CountDownTimer
    public Action OnTimeIsUpEnter = () => { };
    bool timerActive = false;
    bool isContinuous = false;
    public float countDownTime { get; private set; }
    public float countDownTimer { get; private set; }
    
    public GenericTimer(float timeDuration, bool isContinuous)
    {
        SetDuration(timeDuration);
        SetIsContinuous(isContinuous);
    }

    public void SetDuration(float timeDuration)
    {
        countDownTime = timeDuration;
        countDownTimer = countDownTime;
    }

    public void StartTimer()
    {
        SetDuration(countDownTime);
        timerActive = true;
    }

    public void UpdateTimer()
    {
        if (timerActive)
        {
            countDownTimer -= Time.deltaTime;
            if (countDownTimer <= 0)
            {
                InvokeTimer();
                SetDuration(countDownTime);
            }
        }
    }

    private void InvokeTimer()
    {
        OnTimeIsUpEnter.Invoke();
        if (!isContinuous)
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
    #endregion
}
