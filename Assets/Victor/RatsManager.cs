using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatsManager : MonoBehaviour
{
    public bool isRoundActive { get; set; } = false;
    public Spawner[] RatHoles;
    CountDownTimer timer;
    private float timeBeforeSpawnInvoke;
    private Vector2 timeRange = new Vector2(5.0f, 15.0f);
    [HideInInspector] public float timeElapsedRound { get; set; }
    //Use value as percentage paired with timeRange, to make them appear faster as the game goes on
    private float clumsinessFactor = 1.0f;
    //Same as above, more the time passe, more the values goes down and the rats will apear faster
    private float timePassedFactor = 1.0f;

    private void Awake()
    {
        timer = new CountDownTimer(timeRange.x, true);
        timer.OnTimeIsUpLogic += () => { OnTimerInvokeLogic(); };
    }

    public void StartRound()
    {
        isRoundActive = true;
        timer.StartTimer();
    }

    public void EndRound()
    {
        isRoundActive = false;
        timer.EndTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRoundActive)
        {
            timer.UpdateTimer();
            ComputeTimePassedFactor();
        }
    }

    private void OnTimerInvokeLogic()
    {
        Debug.Log("Ding ding");
        SpawnNewRat();
        timeBeforeSpawnInvoke = Random.Range(timeRange.x, timeRange.y) * timePassedFactor;
        timer.SetDuration(timeBeforeSpawnInvoke);
    }

    private void SpawnNewRat()
    {
        int chosenHoleIndex = Random.Range(0, (RatHoles.Length - 1));
        RatHoles[chosenHoleIndex].InvokeSpawn();
    }

    private void ComputeTimePassedFactor()
    {
        if(timeElapsedRound > 60.0f)
            timePassedFactor = 0.9f;
        else if(timeElapsedRound > 120.0f)
            timePassedFactor = 0.8f;
        else if (timeElapsedRound > 180.0f)
            timePassedFactor = 0.7f;
        else if (timeElapsedRound > 240.0f)
            timePassedFactor = 0.6f;
        else if (timeElapsedRound > 300.0f)
            timePassedFactor = 0.5f;
    }
}
