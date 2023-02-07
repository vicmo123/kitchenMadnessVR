using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Respawnable r;
    private GenericTimer timer;
    int wantedNumberOfIterations = 5;

    // Start is called before the first frame update
    void Start()
    {
        timer = new GenericTimer(0.5f, true);
        timer.OnTimeIsUpLogic += () => { OnTimeIsUpLogic(); };
        timer.StartTimer();
        r = gameObject.GetComponent<Respawnable>();
        r.OnRespawnLogic += () => { AdditionalFeaturesSpawnTest(); };
    }

    private void OnTimeIsUpLogic()
    {
        Debug.Log("time is up");
    }

    // Update is called once per frame
    void Update()
    {
        timer.UpdateTimer();
        if(timer.Iterations == wantedNumberOfIterations)
        {
            timer.SetIsContinuous(false);
            Transform t = transform;
            r.InvokeRespawn(t, 2.0f);
            timer.SetDuration(5.0f);
            timer.StartTimer();
            timer.OnTimeIsUpLogic = () => { r.InvokeRespawn(); };
            timer.ResetIterations();
        }
    }

    public static IEnumerator WaitForFrames(int frameCount)
    {
        while (frameCount > 0)
        {
            frameCount--;
            yield return null;
        }
    }

    private void AdditionalFeaturesSpawnTest()
    {
        Debug.Log("Do extra stuff");
    }
}
