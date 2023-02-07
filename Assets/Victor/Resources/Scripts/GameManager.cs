using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GenericTimer timer;
    int counter = 0;
    int wantedNumberOfTicks = 10;

    // Start is called before the first frame update
    void Start()
    {
        timer = new GenericTimer(0.5f, true);
        timer.OnTimeIsUpEnter += () => { OnTimeIsUpLogic(); };
        timer.StartTimer();
    }

    private void OnTimeIsUpLogic()
    {
        Debug.Log("time is up");
        timer.StartTimer();
        counter++;
    }

    // Update is called once per frame
    void Update()
    {
        timer.UpdateTimer();
        if(counter == wantedNumberOfTicks - 1)
        {
            timer.SetIsContinuous(false);
        }
    }
}
