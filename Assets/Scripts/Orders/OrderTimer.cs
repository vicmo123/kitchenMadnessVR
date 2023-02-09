using System;
using UnityEngine;

public class OrderTimer : MonoBehaviour
{
    public Order order;
    private CountDownTimer timer;
    private Color color;
    private float timeDuration = 30;
    private float timePassing = 0;
    private float pourcentage = 100;
    
    void Start()
    {
        color = Color.green;
        timer = new CountDownTimer(timeDuration, false);
        timer.OnTimeIsUpLogic += () => { TimerWentOut(); };
    }

    private void Update()
    {        
        timePassing = timer.CountDownTime;
         pourcentage = timePassing / timeDuration;

        if (pourcentage >= 70)
        {
            color = Color.green;
        }
        else if (pourcentage < 70 && pourcentage >= 40)
        {
            color = Color.yellow;
        }
        else if (pourcentage < 40)
        {
            color = Color.red;
        }
    }

    public void SetDuration(float duration)
    {
        timeDuration = duration;
    }

    public void TimerWentOut()
    {
        order.TimerIsUp = true;
    }
    public Color GetColor()
    {
        return color;
    }
}