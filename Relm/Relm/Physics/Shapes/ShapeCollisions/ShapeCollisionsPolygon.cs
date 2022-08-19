using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;

namespace Relm.Physics.Shapes
{
	public static partial class ShapeCollisions
	{
		public static bool PolygonToPolygon(Polygon first, Polygon second, out CollisionResult result)
		{
			result = new CollisionResult();
			var isIntersecting = true;

			var firstEdges = first.EdgeNormals;
			var secondEdges = second.EdgeNormals;
			var minIntervalDistance = float.PositiveInfinity;
			var translationAxis = new Vector2();
			var polygonOffset = first.position - second.position;
			Vector2 axis;

			for (var edgeIndex = 0; edgeIndex < firstEdges.Length + secondEdges.Length; edgeIndex++)
			{
				if (edgeIndex < firstEdges.Length)
					axis = firstEdges[edgeIndex];
				else
					axis = secondEdges[edgeIndex - firstEdges.Length];

				float minA = 0;
				float minB = 0;
				float maxA = 0;
				float maxB = 0;
				var intervalDist = 0f;
				GetInterval(axis, first, ref minA, ref maxA);
				GetInterval(axis, second, ref minB, ref maxB);

				float relativeIntervalOffset;
				Vector2.Dot(ref polygonOffset, ref axis, out relativeIntervalOffset);
				minA += relativeIntervalOffset;
				maxA += relativeIntervalOffset;

				intervalDist = IntervalDistance(minA, maxA, minB, maxB);
				if (intervalDist > 0)
					isIntersecting = false;


				if (!isIntersecting)
					return false;

				intervalDist = System.Math.Abs(intervalDist);
				if (intervalDist < minIntervalDistance)
				{
					minIntervalDistance = intervalDist;
					translationAxis = axis;

					if (Vector2.Dot(translationAxis, polygonOffset) < 0)
						translationAxis = -translationAxis;
				}
			}

			result.Normal = translationAxis;
			result.MinimumTranslationVector = -translationAxis * minIntervalDistance;

			return true;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static float IntervalDistance(float minA, float maxA, float minB, float maxB)
		{
			if (minA < minB)
				return minB - maxA;

			return minA - maxB;
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static void GetInterval(Vector2 axis, Polygon polygon, ref float min, ref float max)
		{
			float dot;
			Vector2.Dot(ref polygon.Points[0], ref axis, out dot);
			min = max = dot;

			for (var i = 1; i < polygon.Points.Length; i++)
			{
				Vector2.Dot(ref polygon.Points[i], ref axis, out dot);
				if (dot < min)
					min = dot;
				else if (dot > max)
					max = dot;
			}
		}
	}
}