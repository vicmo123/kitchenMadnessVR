using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Spawner[] RatHoles;
    [SerializeField, Range(3.0f, 10.0f)] private float timeBeforeInvoke;
    CountDownTimer timer;

    private void Awake()
    {
        timer = new CountDownTimer(timeBeforeInvoke, true);
        timer.OnTimeIsUpLogic += () => { OnTimerInvokeLogic(); };
    }
    private void Start()
    {
        timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        timer.UpdateTimer();
    }

    private void OnTimerInvokeLogic()
    {
        Debug.Log("Ding ding");
        FindClosestSpawnPoint();
    }

    private void FindClosestSpawnPoint()
    {
        float minDist = Mathf.Infinity;
        Spawner closestRatHole = null;
        foreach (Spawner ratHole in RatHoles)
        {
            float dist = Vector3.Distance(ratHole.transform.position, transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestRatHole = ratHole;
            }
        }

        if (closestRatHole)
        {
            closestRatHole.InvokeSpawn();
        }
    }
}
