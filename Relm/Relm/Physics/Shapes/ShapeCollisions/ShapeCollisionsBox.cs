using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Math;

namespace Relm.Physics.Shapes
{
	public static partial class ShapeCollisions
	{
		public static bool Collide(Shape first, Shape second, Vector2 deltaMovement, out RaycastHit hit)
		{
			hit = new RaycastHit();
			throw new NotImplementedException("this should probably be in each Shape class and it still needs to be implemented ;)");
		}


		public static bool BoxToBoxCast(Box first, Box second, Vector2 movement, out RaycastHit hit)
		{
			hit = new RaycastHit();

			var minkowskiDiff = MinkowskiDifference(first, second);
			if (minkowskiDiff.Contains(0f, 0f))
			{
				var mtv = minkowskiDiff.GetClosestPointOnBoundsToOrigin();
				if (mtv == Vector2.Zero)
					return false;

				hit.Normal = -mtv;
				hit.Normal.Normalize();
				hit.Distance = 0f;
				hit.Fraction = 0f;

				return true;
			}
			else
			{
				var ray = new Ray2D(Vector2.Zero, -movement);
				float fraction;
				if (minkowskiDiff.RayIntersects(ref ray, out fraction) && fraction <= 1.0f)
				{
					hit.Fraction = fraction;
					hit.Distance = movement.Length() * fraction;
					hit.Normal = -movement;
					hit.Normal.Normalize();
					hit.Centroid = first.bounds.Center + movement * fraction;

					return true;
				}
			}

			return false;
		}


		public static bool BoxToBox(Box first, Box second, out CollisionResult result)
		{
			result = new CollisionResult();

			var minkowskiDiff = MinkowskiDifference(first, second);
			if (minkowskiDiff.Contains(0f, 0f))
			{
				// calculate the MTV. if it is zero then we can just call this a non-collision
				result.MinimumTranslationVector = minkowskiDiff.GetClosestPointOnBoundsToOrigin();

				if (result.MinimumTranslationVector == Vector2.Zero)
					return false;

				result.Normal = -result.MinimumTranslationVector;
				result.Normal.Normalize();

				return true;
			}

			return false;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static RectangleF MinkowskiDifference(Box first, Box second)
		{
			var positionOffset = first.position - (first.bounds.Location + first.bounds.Size / 2f);
			var topLeft = first.bounds.Location + positionOffset - second.bounds.Max;
			var fullSize = first.bounds.Size + second.bounds.Size;

			return new RectangleF(topLeft.X, topLeft.Y, fullSize.X, fullSize.Y);
		}
    }
}