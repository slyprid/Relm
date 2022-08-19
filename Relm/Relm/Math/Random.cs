using System;
using Microsoft.Xna.Framework;

namespace Relm.Math
{
	public static class Random
	{
		private static int _seed = Environment.TickCount;
		public static System.Random RND = new System.Random(_seed);
		
        public static int GetSeed()
		{
			return _seed;
		}

        public static void SetSeed(int seed)
		{
			_seed = seed;
			RND = new System.Random(_seed);
		}

        public static float NextFloat()
		{
			return (float)RND.NextDouble();
		}

        public static float NextFloat(float max)
		{
			return (float)RND.NextDouble() * max;
		}

        public static int NextInt(int max)
		{
			return RND.Next(max);
		}

        public static float NextAngle()
		{
			return (float)RND.NextDouble() * MathHelper.TwoPi;
		}

        public static Vector2 NextUnitVector()
		{
			float angle = NextAngle();
			return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
		}

        public static Color NextColor()
		{
			return new Color(NextFloat(), NextFloat(), NextFloat());
		}

        public static int Range(int min, int max)
		{
			return RND.Next(min, max);
		}

        public static float Range(float min, float max)
		{
			return min + NextFloat(max - min);
		}

        public static Vector2 Range(Vector2 min, Vector2 max)
		{
			return min + new Vector2(NextFloat(max.X - min.X), NextFloat(max.Y - min.Y));
		}

        public static float MinusOneToOne()
		{
			return NextFloat(2f) - 1f;
		}

        public static bool Chance(float percent)
		{
			return NextFloat() < percent;
		}

        public static bool Chance(int value)
		{
			return NextInt(100) < value;
		}

        public static T Choose<T>(T first, T second)
		{
			if (NextInt(2) == 0)
				return first;

			return second;
		}

        public static T Choose<T>(T first, T second, T third)
		{
			switch (NextInt(3))
			{
				case 0:
					return first;
				case 1:
					return second;
				default:
					return third;
			}
		}

        public static T Choose<T>(T first, T second, T third, T fourth)
		{
			switch (NextInt(4))
			{
				case 0:
					return first;
				case 1:
					return second;
				case 2:
					return third;
				default:
					return fourth;
			}
		}
	}
}