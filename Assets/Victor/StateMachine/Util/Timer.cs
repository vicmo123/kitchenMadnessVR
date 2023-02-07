using UnityEngine;

namespace FSM
{
	/// <summary>
	/// Default timer that calculates the elapsed time based on Time.time.
	/// </summary>
	public class Timer : ITimer
	{
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
	}
}
