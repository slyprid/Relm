using System;using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Core;

namespace Relm.Math
{
	public static class Mathf
	{
		public const float Epsilon = 0.00001f;
		public const float Deg2Rad = 0.0174532924f;
		public const float Rad2Deg = 57.29578f;


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Round(float f)
		{
			return (float)System.Math.Round(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Ceil(float f)
		{
			return (float)System.Math.Ceiling(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CeilToInt(float f)
		{
			return (int)System.Math.Ceiling((double)f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FastCeilToInt(float y)
		{
			return 32768 - (int)(32768f - y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Floor(float f)
		{
			return (float)System.Math.Floor(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FloorToInt(float f)
		{
			return (int)System.Math.Floor((double)f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Radians(float x) => x * 0.0174532925f;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Degrees(float x) => x * 57.295779513f;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int FastFloorToInt(float x)
		{
			return (int)(x + 32768f) - 32768;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int RoundToInt(float f)
		{
			return (int)System.Math.Round(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int TruncateToInt(float f)
		{
			return (int)System.Math.Truncate(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp01(float value)
		{
			if (value < 0f)
				return 0f;

			if (value > 1f)
				return 1f;

			return value;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Clamp(float value, float min, float max)
		{
			if (value < min)
				return min;

			if (value > max)
				return max;

			return value;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Clamp(int value, int min, int max)
		{
			value = (value > max) ? max : value;
			value = (value < min) ? min : value;

			return value;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Snap(float value, float increment)
		{
			return Round(value / increment) * increment;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Snap(float value, float increment, float offset)
		{
			return (Round((value - offset) / increment) * increment) + offset;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Lerp(float from, float to, float t)
		{
			return from + (to - from) * Clamp01(t);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float InverseLerp(float from, float to, float t)
		{
			if (from < to)
			{
				if (t < from)
					return 0.0f;
				else if (t > to)
					return 1.0f;
			}
			else
			{
				if (t < to)
					return 1.0f;
				else if (t > from)
					return 0.0f;
			}

			return (t - from) / (to - from);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float UnclampedLerp(float from, float to, float t)
		{
			return from + (to - from) * t;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpAngle(float a, float b, float t)
		{
			float num = Repeat(b - a, 360f);
			if (num > 180f)
				num -= 360f;

			return a + num * Clamp01(t);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float LerpAngleRadians(float a, float b, float t)
		{
			float num = Repeat(b - a, MathHelper.TwoPi);
			if (num > MathHelper.Pi)
				num -= MathHelper.TwoPi;

			return a + num * Clamp01(t);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Repeat(float t, float length)
		{
			return t - Floor(t / length) * length;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IncrementWithWrap(int t, int length)
		{
			t++;
			if (t == length)
				return 0;

			return t;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int DecrementWithWrap(int t, int length)
		{
			t--;
			if (t < 0)
				return length - 1;

			return t;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float PingPong(float t, float length)
		{
			t = Repeat(t, length * 2f);
			return length - System.Math.Abs(t - length);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float SignThreshold(float value, float threshold)
		{
			if (System.Math.Abs(value) >= threshold)
				return System.Math.Sign(value);
			else
				return 0;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeltaAngle(float current, float target)
		{
			var num = Repeat(target - current, 360f);
			if (num > 180f)
				num -= 360f;

			return num;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float DeltaAngleRadians(float current, float target)
		{
			var num = Repeat(target - current, 2 * MathHelper.Pi);
			if (num > MathHelper.Pi)
				num -= 2 * MathHelper.Pi;

			return num;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Approach(float start, float end, float shift)
		{
			if (start < end)
				return System.Math.Min(start + shift, end);

			return System.Math.Max(start - shift, end);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ApproachAngle(float start, float end, float shift)
		{
			float deltaAngle = DeltaAngle(start, end);
			if (-shift < deltaAngle && deltaAngle < shift)
				return end;

			return Repeat(Approach(start, start + deltaAngle, shift), 360f);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float ApproachAngleRadians(float start, float end, float shift)
		{
			float deltaAngleRadians = DeltaAngleRadians(start, end);
			if (-shift < deltaAngleRadians && deltaAngleRadians < shift)
				return end;

			return Repeat(Approach(start, start + deltaAngleRadians, shift), MathHelper.TwoPi);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Approximately(float value1, float value2, float tolerance = Epsilon)
		{
			return System.Math.Abs(value1 - value2) <= tolerance;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MinOf(float a, float b, float c)
		{
			return System.Math.Min(a, System.Math.Min(b, c));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MaxOf(float a, float b, float c)
		{
			return System.Math.Max(a, System.Math.Max(b, c));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MinOf(float a, float b, float c, float d)
		{
			return System.Math.Min(a, System.Math.Min(b, System.Math.Min(c, d)));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MinOf(float a, float b, float c, float d, float e)
		{
			return System.Math.Min(a, System.Math.Min(b, System.Math.Min(c, System.Math.Min(d, e))));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MaxOf(float a, float b, float c, float d)
		{
			return System.Math.Max(a, System.Math.Max(b, System.Math.Max(c, d)));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float MaxOf(float a, float b, float c, float d, float e)
		{
			return System.Math.Max(a, System.Math.Max(b, System.Math.Max(c, System.Math.Max(d, e))));
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Between(float value, float min, float max)
		{
			return value >= min && value <= max;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool Between(int value, int min, int max)
		{
			return value >= min && value <= max;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsEven(int value)
		{
			return value % 2 == 0;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsOdd(int value)
		{
			return value % 2 != 0;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundWithRoundedAmount(float value, out float roundedAmount)
		{
			var rounded = Round(value);
			roundedAmount = value - (rounded * Round(value / rounded));
			return rounded;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Map01(float value, float min, float max)
		{
			return (value - min) * 1f / (max - min);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Map10(float value, float min, float max)
		{
			return 1f - Map01(value, min, max);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Map(float value, float leftMin, float leftMax, float rightMin, float rightMax)
		{
			return rightMin + (value - leftMin) * (rightMax - rightMin) / (leftMax - leftMin);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float RoundToNearest(float value, float roundToNearest)
		{
			return Round(value / roundToNearest) * roundToNearest;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool WithinEpsilon(float value, float ep = Epsilon)
		{
			return System.Math.Abs(value) < ep;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Hypotenuse(float x, float y)
		{
			return Sqrt(x * x + y * y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int ClosestPowerOfTwoGreaterThan(int x)
		{
			x--;
			x |= (x >> 1);
			x |= (x >> 2);
			x |= (x >> 4);
			x |= (x >> 8);
			x |= (x >> 16);

			return (x + 1);
		}


		#region wrappers for Math doubles

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sqrt(float val)
		{
			return (float)System.Math.Sqrt(val);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Pow(float x, float y)
		{
			return (float)System.Math.Pow(x, y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Sin(float f)
		{
			return (float)System.Math.Sin(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cos(float f)
		{
			return (float)System.Math.Cos(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Acos(float f)
		{
			return (float)System.Math.Acos(f);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Exp(float power)
		{
			return (float)System.Math.Exp(power);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Atan2(float y, float x)
		{
			return (float)System.Math.Atan2(y, x);
		}

		#endregion


		#region Vector2

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AngleBetweenVectors(Vector2 from, Vector2 to)
		{
			return Atan2(to.Y - from.Y, to.X - from.X);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 AngleToVector(float angleRadians, float length)
		{
			return new Vector2(Cos(angleRadians) * length, Sin(angleRadians) * length);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RotateAround(Vector2 position, float speed)
		{
			var time = Time.TotalTime * speed;

			var x = Cos(time);
			var y = Sin(time);

			return new Vector2(position.X + x, position.Y + y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RotateAround(Vector2 point, Vector2 center, float angleInDegrees)
		{
			angleInDegrees = MathHelper.ToRadians(angleInDegrees);
			var cos = Cos(angleInDegrees);
			var sin = Sin(angleInDegrees);
			var rotatedX = cos * (point.X - center.X) - sin * (point.Y - center.Y) + center.X;
			var rotatedY = sin * (point.X - center.X) + cos * (point.Y - center.Y) + center.Y;

			return new Vector2(rotatedX, rotatedY);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 RotateAroundRadians(Vector2 point, Vector2 center, float angleInRadians)
		{
			var cos = Cos(angleInRadians);
			var sin = Sin(angleInRadians);
			var rotatedX = cos * (point.X - center.X) - sin * (point.Y - center.Y) + center.X;
			var rotatedY = sin * (point.X - center.X) + cos * (point.Y - center.Y) + center.Y;

			return new Vector2(rotatedX, rotatedY);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 PointOnCircle(Vector2 circleCenter, float radius, float angleInDegrees)
		{
			var radians = MathHelper.ToRadians(angleInDegrees);
			return new Vector2
			{
				X = Cos(radians) * radius + circleCenter.X,
				Y = Sin(radians) * radius + circleCenter.Y
			};
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 PointOnCircleRadians(Vector2 circleCenter, float radius, float angleInRadians)
		{
			return new Vector2
			{
				X = Cos(angleInRadians) * radius + circleCenter.X,
				Y = Sin(angleInRadians) * radius + circleCenter.Y
			};
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Lissajou(float xFrequency = 2f, float yFrequency = 3f, float xMagnitude = 1,
									   float yMagnitude = 1, float phase = 0)
		{
			var x = Sin(Time.TotalTime * xFrequency + phase) * xMagnitude;
			var y = Cos(Time.TotalTime * yFrequency) * yMagnitude;

			return new Vector2(x, y);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 LissajouDamped(float xFrequency = 2f, float yFrequency = 3f, float xMagnitude = 1,
											 float yMagnitude = 1, float phase = 0.5f, float damping = 0f,
											 float oscillationInterval = 5f)
		{
			var wrappedTime = PingPong(Time.TotalTime, oscillationInterval);
			var damped = Pow(MathHelper.E, -damping * wrappedTime);

			var x = damped * Sin(Time.TotalTime * xFrequency + phase) * xMagnitude;
			var y = damped * Cos(Time.TotalTime * yFrequency) * yMagnitude;

			return new Vector2(x, y);
		}

		#endregion
	}
}