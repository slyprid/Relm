using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Physics.Shapes
{
	public static partial class ShapeCollisions
	{
		public static bool CircleToCircleCast(Circle first, Circle second, Vector2 deltaMovement, out RaycastHit hit)
		{
			hit = new RaycastHit();

			var endPointOfCast = first.position + deltaMovement;
			var d = ClosestPointOnLine(first.position, endPointOfCast, second.position);

			var closestDistanceSquared = Vector2.DistanceSquared(second.position, d);
			var sumOfRadiiSquared = (first.Radius + second.Radius) * (first.Radius + second.Radius);

			if (closestDistanceSquared <= sumOfRadiiSquared)
			{
				var normalizedDeltaMovement = Vector2.Normalize(deltaMovement);

				if (d == endPointOfCast)
				{
					endPointOfCast = first.position + deltaMovement + normalizedDeltaMovement * second.Radius;
					d = ClosestPointOnLine(first.position, endPointOfCast, second.position);
					closestDistanceSquared = Vector2.DistanceSquared(second.position, d);
				}

				var backDist = Mathf.Sqrt(sumOfRadiiSquared - closestDistanceSquared);

				hit.Centroid = d - backDist * normalizedDeltaMovement;
				hit.Normal = Vector2.Normalize(hit.Centroid - second.position);
				hit.Fraction = (hit.Centroid.X - first.position.X) / deltaMovement.X;
				Vector2.Distance(ref first.position, ref hit.Centroid, out hit.Distance);
				hit.Point = second.position + hit.Normal * second.Radius;

				return true;
			}

			return false;
		}


		public static bool CircleToCircle(Circle first, Circle second, out CollisionResult result)
		{
			result = new CollisionResult();

			var distanceSquared = Vector2.DistanceSquared(first.position, second.position);
			var sumOfRadii = first.Radius + second.Radius;
			var collided = distanceSquared < sumOfRadii * sumOfRadii;
			if (collided)
			{
				result.Normal = Vector2.Normalize(first.position - second.position);
				var depth = sumOfRadii - Mathf.Sqrt(distanceSquared);
				result.MinimumTranslationVector = -depth * result.Normal;
				result.Point = second.position + result.Normal * second.Radius;
				
				return true;
			}

			return false;
		}
		
		public static bool CircleToBox(Circle circle, Box box, out CollisionResult result)
		{
			result = new CollisionResult();

			var closestPointOnBounds =
				box.bounds.GetClosestPointOnRectangleBorderToPoint(circle.position, out result.Normal);

			if (box.ContainsPoint(circle.position))
			{
				result.Point = closestPointOnBounds;

				var safePlace = closestPointOnBounds + result.Normal * circle.Radius;
				result.MinimumTranslationVector = circle.position - safePlace;

				return true;
			}

			float sqrDistance;
			Vector2.DistanceSquared(ref closestPointOnBounds, ref circle.position, out sqrDistance);

			if (sqrDistance == 0)
			{
				result.MinimumTranslationVector = result.Normal * circle.Radius;
			}
			else if (sqrDistance <= circle.Radius * circle.Radius)
			{
				result.Normal = circle.position - closestPointOnBounds;
				var depth = result.Normal.Length() - circle.Radius;

				result.Point = closestPointOnBounds;
				Vector2Extensions.Normalize(ref result.Normal);
				result.MinimumTranslationVector = depth * result.Normal;

				return true;
			}

			return false;
		}


		public static bool CircleToPolygon(Circle circle, Polygon polygon, out CollisionResult result)
		{
			result = new CollisionResult();

			var poly2Circle = circle.position - polygon.position;

			float distanceSquared;
			var closestPoint = Polygon.GetClosestPointOnPolygonToPoint(polygon.Points, poly2Circle, out distanceSquared,
				out result.Normal);

			var circleCenterInsidePoly = polygon.ContainsPoint(circle.position);
			if (distanceSquared > circle.Radius * circle.Radius && !circleCenterInsidePoly)
				return false;

			Vector2 mtv;
			if (circleCenterInsidePoly)
			{
				mtv = result.Normal * (Mathf.Sqrt(distanceSquared) - circle.Radius);
			}
			else
			{
				if (distanceSquared == 0)
				{
					mtv = result.Normal * circle.Radius;
				}
				else
				{
					var distance = Mathf.Sqrt(distanceSquared);
					mtv = -(poly2Circle - closestPoint) * ((circle.Radius - distance) / distance);
				}
			}

			result.MinimumTranslationVector = mtv;
			result.Point = closestPoint + polygon.position;

			return true;
		}


		static bool CircleToPolygon2(Circle circle, Polygon polygon, out CollisionResult result)
		{
			result = new CollisionResult();

			var closestPointIndex = -1;
			var poly2Circle = circle.position - polygon.position;
			var poly2CircleNormalized = Vector2.Normalize(poly2Circle);
			var max = float.MinValue;

			for (var i = 0; i < polygon.Points.Length; i++)
			{
				var projection = Vector2.Dot(polygon.Points[i], poly2CircleNormalized);
				if (max < projection)
				{
					closestPointIndex = i;
					max = projection;
				}
			}

			var poly2CircleLength = poly2Circle.Length();
			if (poly2CircleLength - max - circle.Radius > 0 && poly2CircleLength > 0)
				return false;

			var prePointIndex = closestPointIndex - 1;
			var postPointIndex = closestPointIndex + 1;

			if (prePointIndex < 0)
				prePointIndex = polygon.Points.Length - 1;

			if (postPointIndex == polygon.Points.Length)
				postPointIndex = 0;

			var circleCenter = circle.position - polygon.position;
			var closest1 = ClosestPointOnLine(polygon.Points[prePointIndex], polygon.Points[closestPointIndex],
				circleCenter);
			var closest2 = ClosestPointOnLine(polygon.Points[closestPointIndex], polygon.Points[postPointIndex],
				circleCenter);
			float distance1, distance2;
			Vector2.DistanceSquared(ref circleCenter, ref closest1, out distance1);
			Vector2.DistanceSquared(ref circleCenter, ref closest2, out distance2);

			var radiusSquared = circle.Radius * circle.Radius;

			float seperationDistance;
			if (distance1 < distance2)
			{
				if (distance1 > radiusSquared)
					return false;

				seperationDistance = circle.Radius - Mathf.Sqrt(distance1);
				var edge = polygon.Points[closestPointIndex] - polygon.Points[prePointIndex];
				result.Normal = new Vector2(edge.Y, -edge.X);
				result.Point = polygon.position + closest1;
			}
			else
			{
				if (distance2 > radiusSquared)
					return false;

				seperationDistance = circle.Radius - Mathf.Sqrt(distance2);
				var edge = polygon.Points[postPointIndex] - polygon.Points[closestPointIndex];
				result.Normal = new Vector2(edge.Y, -edge.X);
				result.Point = polygon.position + closest2;
			}

			result.Normal.Normalize();
			result.MinimumTranslationVector = result.Normal * -seperationDistance;

			return true;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ClosestPointOnLine(Vector2 lineA, Vector2 lineB, Vector2 closestTo)
		{
			var v = lineB - lineA;
			var w = closestTo - lineA;
			var t = Vector2.Dot(w, v) / Vector2.Dot(v, v);
			t = MathHelper.Clamp(t, 0, 1);

			return lineA + v * t;
		}
	}
}