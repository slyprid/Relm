using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Math;

namespace Relm.Extensions
{
    public static class Vector2Extensions
    {
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Normalize(ref Vector2 vec)
		{
			var magnitude = Mathf.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
			if (magnitude > Mathf.Epsilon)
				vec /= magnitude;
			else
				vec.X = vec.Y = 0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Normalize(Vector2 vec)
		{
			var magnitude = Mathf.Sqrt((vec.X * vec.X) + (vec.Y * vec.Y));
			if (magnitude > Mathf.Epsilon)
				vec /= magnitude;
			else
				vec.X = vec.Y = 0;

			return vec;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Round(this Vector2 vec)
		{
			return new Vector2(Mathf.Round(vec.X), Mathf.Round(vec.Y));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Round(ref Vector2 vec)
		{
			vec.X = Mathf.Round(vec.X);
			vec.Y = Mathf.Round(vec.Y);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Floor(ref Vector2 val)
		{
			val.X = (int)val.X;
			val.Y = (int)val.Y;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Floor(Vector2 val)
		{
			return new Vector2((int)val.X, (int)val.Y);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 HalfVector()
		{
			return new Vector2(0.5f, 0.5f);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Cross(Vector2 u, Vector2 v)
		{
			return u.Y * v.X - u.X * v.Y;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(ref Vector2 first, ref Vector2 second)
		{
			return new Vector2(-1f * (second.Y - first.Y), second.X - first.X);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 first, Vector2 second)
		{
			return new Vector2(-1f * (second.Y - first.Y), second.X - first.X);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Perpendicular(Vector2 original)
		{
			return new Vector2(-original.Y, original.X);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float Angle(Vector2 from, Vector2 to)
		{
			Normalize(ref from);
			Normalize(ref to);
			return Mathf.Acos(Mathf.Clamp(Vector2.Dot(from, to), -1f, 1f)) * Mathf.Rad2Deg;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float AngleBetween(this Vector2 self, Vector2 left, Vector2 right)
		{
			var one = left - self;
			var two = right - self;
			return Angle(one, two);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool GetRayIntersection(Vector2 a, Vector2 b, Vector2 c, Vector2 d, out Vector2 intersection)
		{
			var dy1 = b.Y - a.Y;
			var dx1 = b.X - a.X;
			var dy2 = d.Y - c.Y;
			var dx2 = d.X - c.X;

			if (dy1 * dx2 == dy2 * dx1)
			{
				intersection = new Vector2(float.NaN, float.NaN);
				return false;
			}

			var x = ((c.Y - a.Y) * dx1 * dx2 + dy1 * dx2 * a.X - dy2 * dx1 * c.X) / (dy1 * dx2 - dy2 * dx1);
			var y = a.Y + (dy1 / dx1) * (x - a.X);

			intersection = new Vector2(x, y);
			return true;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Point RoundToPoint(this Vector2 vec)
		{
			var roundedVec = Vector2Extensions.Round(vec);
			return new Point((int)roundedVec.X, (int)roundedVec.Y);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector3 ToVector3(this Vector2 vec)
		{
			return new Vector3(vec, 0);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsTriangleCCW(Vector2 a, Vector2 center, Vector2 c)
		{
			return Cross(center - a, c - center) < 0;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 Transform(Vector2 position, Matrix2D matrix)
		{
			return new Vector2((position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M31,
				(position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M32);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Transform(ref Vector2 position, ref Matrix2D matrix, out Vector2 result)
		{
			var x = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M31;
			var y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M32;
			result.X = x;
			result.Y = y;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix2D matrix,
									 Vector2[] destinationArray, int destinationIndex, int length)
		{
			for (var i = 0; i < length; i++)
			{
				var position = sourceArray[sourceIndex + i];
				var destination = destinationArray[destinationIndex + i];
				destination.X = (position.X * matrix.M11) + (position.Y * matrix.M21) + matrix.M31;
				destination.Y = (position.X * matrix.M12) + (position.Y * matrix.M22) + matrix.M32;
				destinationArray[destinationIndex + i] = destination;
			}
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void Transform(Vector2[] sourceArray, ref Matrix2D matrix, Vector2[] destinationArray)
		{
			Transform(sourceArray, 0, ref matrix, destinationArray, 0, sourceArray.Length);
		}

        public static Vector2 MinValue(this Vector2 input)
        {
            return new Vector2(float.MinValue, float.MinValue);
        }
	}
}