using Microsoft.Xna.Framework;
using Relm.Core;
using Relm.Math;

namespace Relm.Graphics.Tweening
{
	public static class Lerps
	{
		#region Lerps

		public static float Lerp(float from, float to, float t)
		{
			return from + (to - from) * t;
		}
		
		public static float LerpTowards(float from, float to, float remainingFactorPerSecond, float deltaTime)
		{
			return Lerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}
		
		public static float LerpDamp(float source, float target, float smoothing)
		{
			return MathHelper.Lerp(source, target, 1 - Mathf.Pow(smoothing, Time.DeltaTime));
		}
		
		public static Vector2 Lerp(Vector2 from, Vector2 to, float t)
		{
			return new Vector2(from.X + (to.X - from.X) * t, from.Y + (to.Y - from.Y) * t);
		}
		
		public static Vector2 LerpTowards(Vector2 from, Vector2 to, float remainingFactorPerSecond, float deltaTime)
		{
			return Lerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}
		public static Vector3 Lerp(Vector3 from, Vector3 to, float t)
		{
			return new Vector3(from.X + (to.X - from.X) * t, from.Y + (to.Y - from.Y) * t,
				from.Z + (to.Z - from.Z) * t);
		}
		
		public static Vector3 LerpTowards(Vector3 from, Vector3 to, float remainingFactorPerSecond, float deltaTime)
		{
			return Lerp(from, to, 1f - Mathf.Pow(remainingFactorPerSecond, deltaTime));
		}
		
		public static Vector3 LerpTowards(Vector3 followerCurrentPosition, Vector3 targetPreviousPosition,
										  Vector3 targetCurrentPosition, float smoothFactor, float deltaTime)
		{
			var targetDiff = targetCurrentPosition - targetPreviousPosition;
			var temp = followerCurrentPosition - targetPreviousPosition + targetDiff / (smoothFactor * deltaTime);
			return targetCurrentPosition - targetDiff / (smoothFactor * deltaTime) + temp * Mathf.Exp(-smoothFactor * deltaTime);
		}
		
		public static Vector2 AngleLerp(Vector2 from, Vector2 to, float t)
		{
			var toMinusFrom = new Vector2(Mathf.DeltaAngle(from.X, to.X), Mathf.DeltaAngle(from.Y, to.Y));
			return new Vector2(from.X + toMinusFrom.X * t, from.Y + toMinusFrom.Y * t);
		}
		
		public static Vector4 Lerp(Vector4 from, Vector4 to, float t)
		{
			return new Vector4(from.X + (to.X - from.X) * t, from.Y + (to.Y - from.Y) * t, from.Z + (to.Z - from.Z) * t,
				from.W + (to.W - from.W) * t);
		}
		
		public static Color Lerp(Color from, Color to, float t)
		{
			var t255 = (int)(t * 255);
			return new Color(from.R + (to.R - from.R) * t255 / 255, from.G + (to.G - from.G) * t255 / 255, from.B + (to.B - from.B) * t255 / 255, from.A + (to.A - from.A) * t255 / 255);
		}
		
		public static Color Lerp(ref Color from, ref Color to, float t)
		{
			var t255 = (int)(t * 255);
			return new Color(from.R + (to.R - from.R) * t255 / 255, from.G + (to.G - from.G) * t255 / 255, from.B + (to.B - from.B) * t255 / 255, from.A + (to.A - from.A) * t255 / 255);
		}
		public static Rectangle Lerp(Rectangle from, Rectangle to, float t)
		{
			return new Rectangle
			(
				(int)(from.X + (to.X - from.X) * t),
				(int)(from.Y + (to.Y - from.Y) * t),
				(int)(from.Width + (to.Width - from.Width) * t),
				(int)(from.Height + (to.Height - from.Height) * t)
			);
		}
		public static Rectangle Lerp(ref Rectangle from, ref Rectangle to, float t)
		{
			return new Rectangle
			(
				(int)(from.X + (to.X - from.X) * t),
				(int)(from.Y + (to.Y - from.Y) * t),
				(int)(from.Width + (to.Width - from.Width) * t),
				(int)(from.Height + (to.Height - from.Height) * t)
			);
		}

		#endregion

        #region Easers

		public static float Ease(EaseType easeType, float from, float to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Vector2 Ease(EaseType easeType, Vector2 from, Vector2 to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Vector3 Ease(EaseType easeType, Vector3 from, Vector3 to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Vector2 EaseAngle(EaseType easeType, Vector2 from, Vector2 to, float t, float duration)
		{
			return AngleLerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Vector4 Ease(EaseType easeType, Vector4 from, Vector4 to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Quaternion Ease(EaseType easeType, Quaternion from, Quaternion to, float t, float duration)
		{
			return Quaternion.Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Color Ease(EaseType easeType, Color from, Color to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Color Ease(EaseType easeType, ref Color from, ref Color to, float t, float duration)
		{
			return Lerp(ref from, ref to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Rectangle Ease(EaseType easeType, Rectangle from, Rectangle to, float t, float duration)
		{
			return Lerp(from, to, EaseHelper.Ease(easeType, t, duration));
		}

        public static Rectangle Ease(EaseType easeType, ref Rectangle from, ref Rectangle to, float t, float duration)
		{
			return Lerp(ref from, ref to, EaseHelper.Ease(easeType, t, duration));
		}

		#endregion
		
		#region Springs

		public static float FastSpring(float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency)
		{
			velocity += -2.0f * Time.DeltaTime * dampingRatio * angularFrequency * velocity + Time.DeltaTime * angularFrequency * angularFrequency * (targetValue - currentValue);
			currentValue += Time.DeltaTime * velocity;

			return currentValue;
		}
		
		public static float StableSpring(float currentValue, float targetValue, ref float velocity, float dampingRatio, float angularFrequency)
		{
			var f = 1f + 2f * Time.DeltaTime * dampingRatio * angularFrequency;
			var oo = angularFrequency * angularFrequency;
			var hoo = Time.DeltaTime * oo;
			var hhoo = Time.DeltaTime * hoo;
			var detInv = 1.0f / (f + hhoo);
			var detX = f * currentValue + Time.DeltaTime * velocity + hhoo * targetValue;
			var detV = velocity + hoo * (targetValue - currentValue);

			currentValue = detX * detInv;
			velocity = detV * detInv;

			return currentValue;
		}
		
		public static Vector2 FastSpring(Vector2 currentValue, Vector2 targetValue, ref Vector2 velocity, float dampingRatio, float angularFrequency)
		{
			velocity += -2.0f * Time.DeltaTime * dampingRatio * angularFrequency * velocity + Time.DeltaTime * angularFrequency * angularFrequency * (targetValue - currentValue);
			currentValue += Time.DeltaTime * velocity;

			return currentValue;
		}
		
		public static Vector2 StableSpring(Vector2 currentValue, Vector2 targetValue, ref Vector2 velocity, float dampingRatio, float angularFrequency)
		{
			var f = 1f + 2f * Time.DeltaTime * dampingRatio * angularFrequency;
			var oo = angularFrequency * angularFrequency;
			var hoo = Time.DeltaTime * oo;
			var hhoo = Time.DeltaTime * hoo;
			var detInv = 1.0f / (f + hhoo);
			var detX = f * currentValue + Time.DeltaTime * velocity + hhoo * targetValue;
			var detV = velocity + hoo * (targetValue - currentValue);

			currentValue = detX * detInv;
			velocity = detV * detInv;

			return currentValue;
		}

		#endregion
	}
}