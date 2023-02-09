using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic timer that calculates the elapsed time based on Time.time.
/// </summary>
public class Timer
{
    #region chronometer
    public float StartTime;
    public float Elapsed => Time.time - StartTime;

    public Timer()
    {
        StartTime = Time.time;
    }

    public void Reset()
    {
        StartTime = Time.time;
    }

    public static bool operator >(Timer timer, float duration)
        => timer.Elapsed > duration;

    public static bool operator <(Timer timer, float duration)
        => timer.Elapsed < duration;

    public static bool operator >=(Timer timer, float duration)
        => timer.Elapsed >= duration;

    public static bool operator <=(Timer timer, float duration)
        => timer.Elapsed <= duration;
    #endregion
}
