using System;
using UnityEngine;

public class OrderTimer
{
    public CountDownTimer timer;
    private Color color;
    private float timeDuration = 8;
    private float timeRemaining = 0;
    private float pourcentage = 100;

    public OrderTimer(float timeDuration)
    {
        timer = new CountDownTimer(timeDuration, false);
        color = Color.green;
   
    }

    public void StartTimer()
    {
        Debug.Log("Timer starts");
        timer.StartTimer();
    }

    public void UpdateTimer()
    {
        timer.UpdateTimer();
        timeRemaining = timer.Timer;
        pourcentage = timeRemaining / timeDuration;
       if(timeRemaining < 2)
        {
            Debug.Log(timeRemaining);
        }

        if (pourcentage >= 70)
        {
            color = Color.green;
            Debug.Log("Green");
        }
        else if (pourcentage < 70 && pourcentage >= 40)
        {
            color = Color.yellow;
            Debug.Log("Yellow");
        }
        else if (pourcentage < 40)
        {
            color = Color.red;
            Debug.Log("Red");
        }
    }

    public void SetDuration(float duration)
    {
        timeDuration = duration;
    }

    public Color GetColor()
    {
        return color;
    }    

    public float GetTimeLeft()
    {
        return timeRemaining;
    }    
}