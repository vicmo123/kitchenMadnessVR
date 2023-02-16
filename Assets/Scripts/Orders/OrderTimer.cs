using System;
using UnityEngine;

public class OrderTimer
{
    public CountDownTimer timer;
    private Color color;
    private float timeDuration ;
    private float timeRemaining ;
    private float pourcentage;

    public OrderTimer(float timeDuration)
    {
        this.timeDuration = timeDuration;
        timer = new CountDownTimer(timeDuration, false);
        timeRemaining = timer.Timer;
        pourcentage = timeRemaining / timeDuration;
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
       
    }

    public void SetDuration(float duration)
    {
        timeDuration = duration;
    }

    public Color GetColor()
    {
        return color;
    }  
    public void SetColor(Color color)
    {
        this.color = color;
    }

    public float GetPourcentageLeft()
    {
        return timeRemaining;
    }    
}