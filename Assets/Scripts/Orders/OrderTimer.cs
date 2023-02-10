using System;
using UnityEngine;

public class OrderTimer : MonoBehaviour
{
    public Action TimerIstOut = () => { };
    private Order order;
    private CountDownTimer timer;
    private Color color;
    private float timeDuration = 5;
    private float timeRemaining = 0;
    private float pourcentage = 100;
    private void Awake()
    {
        timer = new CountDownTimer(timeDuration, false);        
    }
    void Start()
    {
        color = Color.green;

        if (order.GetIsInUse())
        {
            timer.StartTimer();
        }
        timer.OnTimeIsUpLogic += () => { TimerWentOut(); };
    }

    private void Update()
    {
        timer.UpdateTimer();
        timeRemaining = timer.Timer;
        pourcentage = timeRemaining / timeDuration;

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
        TimerIstOut.Invoke();
    }
    public Color GetColor()
    {
        return color;
    }

    public void SetOrder(Order orderInBoard)
    {
        order = orderInBoard;
    }

    public float GetTimeLeft()
    {
        return timeRemaining;
    }

    public void StartTimer()
    {
        timer.StartTimer();
    }
}