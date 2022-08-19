using System.Runtime.CompilerServices;

namespace Relm.Core
{
	public static class Time
	{
		public static float TotalTime;

		public static float DeltaTime;

		public static float UnscaledDeltaTime;

		public static float AltDeltaTime;

		public static float TimeSinceSceneLoad;

		public static float TimeScale = 1f;

		public static float AltTimeScale = 1f;

		public static uint FrameCount;

		public static float MaxDeltaTime = float.MaxValue;

		internal static void Update(float dt)
		{
			if (dt > MaxDeltaTime) dt = MaxDeltaTime;
			TotalTime += dt;
			DeltaTime = dt * TimeScale;
			AltDeltaTime = dt * AltTimeScale;
			UnscaledDeltaTime = dt;
			TimeSinceSceneLoad += dt;
			FrameCount++;
		}


		internal static void SceneChanged()
		{
			TimeSinceSceneLoad = 0f;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool CheckEvery(float interval)
		{
			return (int)(TimeSinceSceneLoad / interval) > (int)((TimeSinceSceneLoad - DeltaTime) / interval);
		}
	}
}
