using System;
using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Physics.Shapes
{
	public class Polygon 
        : Shape
	{
		public Vector2[] Points;

		public Vector2[] EdgeNormals
		{
			get
			{
				if (_areEdgeNormalsDirty)
					BuildEdgeNormals();
				return _edgeNormals;
			}
		}

		bool _areEdgeNormalsDirty = true;
		public Vector2[] _edgeNormals;

		internal Vector2[] _originalPoints;
		internal Vector2 _polygonCenter;

		internal bool isBox;
		public bool IsUnrotated = true;


		public Polygon(Vector2[] points) => SetPoints(points);

		public Polygon(int vertCount, float radius) => SetPoints(BuildSymmetricalPolygon(vertCount, radius));

		internal Polygon(Vector2[] points, bool isBox) : this(points)
		{
			this.isBox = isBox;
		}

		public void SetPoints(Vector2[] points)
		{
			Points = points;
			RecalculateCenterAndEdgeNormals();

			_originalPoints = new Vector2[points.Length];
			Array.Copy(points, _originalPoints, points.Length);
		}

		public void RecalculateCenterAndEdgeNormals()
		{
			_polygonCenter = FindPolygonCenter(Points);
			_areEdgeNormalsDirty = true;
		}

		void BuildEdgeNormals()
		{
			var totalEdges = isBox ? 2 : Points.Length;
			if (_edgeNormals == null || _edgeNormals.Length != totalEdges)
				_edgeNormals = new Vector2[totalEdges];

			Vector2 p2;
			for (var i = 0; i < totalEdges; i++)
			{
				var p1 = Points[i];
				if (i + 1 >= Points.Length)
					p2 = Points[0];
				else
					p2 = Points[i + 1];

				var perp = Vector2Extensions.Perpendicular(ref p1, ref p2);
                Vector2Extensions.Normalize(ref perp);
				_edgeNormals[i] = perp;
			}

			return;
		}
		
		#region static Polygon helpers

		public static Vector2[] BuildSymmetricalPolygon(int vertCount, float radius)
		{
			var verts = new Vector2[vertCount];

			for (var i = 0; i < vertCount; i++)
			{
				var a = 2.0f * MathHelper.Pi * (i / (float)vertCount);
				verts[i] = new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
			}

			return verts;
		}

		public static void RecenterPolygonVerts(Vector2[] points)
		{
			var center = FindPolygonCenter(points);
			for (var i = 0; i < points.Length; i++)
				points[i] -= center;
		}

		public static Vector2 FindPolygonCenter(Vector2[] points)
		{
			float x = 0, y = 0;

			for (var i = 0; i < points.Length; i++)
			{
				x += points[i].X;
				y += points[i].Y;
			}

			return new Vector2(x / points.Length, y / points.Length);
		}

		public static Vector2 GetFarthestPointInDirection(Vector2[] points, Vector2 direction)
		{
			var index = 0;
			Vector2.Dot(ref points[index], ref direction, out float maxDot);

			for (var i = 1; i < points.Length; i++)
			{
				Vector2.Dot(ref points[i], ref direction, out float dot);
				if (dot > maxDot)
				{
					maxDot = dot;
					index = i;
				}
			}

			return points[index];
		}

		public static Vector2 GetClosestPointOnPolygonToPoint(Vector2[] points, Vector2 point,
															  out float distanceSquared, out Vector2 edgeNormal)
		{
			distanceSquared = float.MaxValue;
			edgeNormal = Vector2.Zero;
			var closestPoint = Vector2.Zero;

			float tempDistanceSquared;
			for (var i = 0; i < points.Length; i++)
			{
				var j = i + 1;
				if (j == points.Length)
					j = 0;

				var closest = ShapeCollisions.ClosestPointOnLine(points[i], points[j], point);
				Vector2.DistanceSquared(ref point, ref closest, out tempDistanceSquared);

				if (tempDistanceSquared < distanceSquared)
				{
					distanceSquared = tempDistanceSquared;
					closestPoint = closest;

					var line = points[j] - points[i];
					edgeNormal.X = -line.Y;
					edgeNormal.Y = line.X;
				}
			}

			Vector2Extensions.Normalize(ref edgeNormal);

			return closestPoint;
		}

		public static void RotatePolygonVerts(float radians, Vector2[] originalPoints, Vector2[] rotatedPoints)
		{
			var cos = Mathf.Cos(radians);
			var sin = Mathf.Sin(radians);

			for (var i = 0; i < originalPoints.Length; i++)
			{
				var position = originalPoints[i];
				rotatedPoints[i] = new Vector2((position.X * cos + position.Y * -sin),
					(position.X * sin + position.Y * cos));
			}
		}

		#endregion


		#region Shape abstract methods

		internal override void RecalculateBounds(Collider collider)
		{
			center = collider.LocalOffset;

			if (collider.ShouldColliderScaleAndRotateWithTransform)
			{
				var hasUnitScale = true;
				Matrix2D tempMat;
				var combinedMatrix = Matrix2D.CreateTranslation(-_polygonCenter);

				if (collider.Entity.Transform.Scale != Vector2.One)
				{
					Matrix2D.CreateScale(collider.Entity.Transform.Scale.X, collider.Entity.Transform.Scale.Y, out tempMat);
					Matrix2D.Multiply(ref combinedMatrix, ref tempMat, out combinedMatrix);

					hasUnitScale = false;

					var scaledOffset = collider.LocalOffset * collider.Entity.Transform.Scale;
					center = scaledOffset;
				}

				if (collider.Entity.Transform.Rotation != 0)
				{
					Matrix2D.CreateRotation(collider.Entity.Transform.Rotation, out tempMat);
					Matrix2D.Multiply(ref combinedMatrix, ref tempMat, out combinedMatrix);

					var offsetAngle = Mathf.Atan2(collider.LocalOffset.Y * collider.Entity.Transform.Scale.Y, collider.LocalOffset.X * collider.Entity.Transform.Scale.X) * Mathf.Rad2Deg;
					var offsetLength = hasUnitScale
						? collider._localOffsetLength
						: (collider.LocalOffset * collider.Entity.Transform.Scale).Length();
					center = Mathf.PointOnCircle(Vector2.Zero, offsetLength,
						collider.Entity.Transform.RotationDegrees + offsetAngle);
				}

				Matrix2D.CreateTranslation(ref _polygonCenter, out tempMat);
				Matrix2D.Multiply(ref combinedMatrix, ref tempMat, out combinedMatrix);

				Vector2Extensions.Transform(_originalPoints, ref combinedMatrix, Points);

				IsUnrotated = collider.Entity.Transform.Rotation == 0;

				if (collider._isRotationDirty)
					_areEdgeNormalsDirty = true;
			}

			position = collider.Entity.Transform.Position + center;
			bounds = RectangleF.RectEncompassingPoints(Points);
			bounds.Location += position;
		}

		public override bool Overlaps(Shape other)
		{
			CollisionResult result;
			if (other is Polygon)
				return ShapeCollisions.PolygonToPolygon(this, other as Polygon, out result);

			if (other is Circle)
			{
				if (ShapeCollisions.CircleToPolygon(other as Circle, this, out result))
				{
					result.InvertResult();
					return true;
				}

				return false;
			}

			throw new NotImplementedException($"Overlaps of Polygon to {other} are not supported");
		}

		public override bool CollidesWithShape(Shape other, out CollisionResult result)
		{
			if (other is Polygon)
				return ShapeCollisions.PolygonToPolygon(this, other as Polygon, out result);

			if (other is Circle)
			{
				if (ShapeCollisions.CircleToPolygon(other as Circle, this, out result))
				{
					result.InvertResult();
					return true;
				}

				return false;
			}

			throw new NotImplementedException($"Overlaps of Polygon to {other} are not supported");
		}

		public override bool CollidesWithLine(Vector2 start, Vector2 end, out RaycastHit hit)
		{
			hit = new RaycastHit();
			return ShapeCollisions.LineToPoly(start, end, this, out hit);
		}

		public override bool ContainsPoint(Vector2 point)
		{
			point -= position;

			var isInside = false;
			for (int i = 0, j = Points.Length - 1; i < Points.Length; j = i++)
			{
				if (((Points[i].Y > point.Y) != (Points[j].Y > point.Y)) &&
					(point.X < (Points[j].X - Points[i].X) * (point.Y - Points[i].Y) / (Points[j].Y - Points[i].Y) +
					 Points[i].X))
				{
					isInside = !isInside;
				}
			}

			return isInside;
		}

		public override bool PointCollidesWithShape(Vector2 point, out CollisionResult result)
		{
			return ShapeCollisions.PointToPoly(point, this, out result);
		}

		#endregion
	}
}