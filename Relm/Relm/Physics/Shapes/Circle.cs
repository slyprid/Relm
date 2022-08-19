using System;
using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Math;

namespace Relm.Physics.Shapes
{
	public class Circle 
        : Shape
	{
		public float Radius;
		internal float _originalRadius;

        public Circle(float radius)
		{
			Radius = radius;
			_originalRadius = radius;
		}

        #region Shape abstract methods

		internal void RecalculateBounds(float radius, Vector2 position)
		{
			_originalRadius = radius;
			Radius = radius;
			this.position = position;
			bounds = new RectangleF(position.X - radius, position.Y - radius, radius * 2f, radius * 2f);
		}


		internal override void RecalculateBounds(Collider collider)
		{
			center = collider.LocalOffset;

			if (collider.ShouldColliderScaleAndRotateWithTransform)
			{
				var scale = collider.Entity.Transform.Scale;
				var hasUnitScale = scale.X == 1 && scale.Y == 1;
				var maxScale = System.Math.Max(scale.X, scale.Y);
				Radius = _originalRadius * maxScale;

				if (collider.Entity.Transform.Rotation != 0)
				{
					var offsetAngle = Mathf.Atan2(collider.LocalOffset.Y, collider.LocalOffset.X) * Mathf.Rad2Deg;
					var offsetLength = hasUnitScale
						? collider._localOffsetLength
						: (collider.LocalOffset * collider.Entity.Transform.Scale).Length();
					center = Mathf.PointOnCircle(Vector2.Zero, offsetLength,
						collider.Entity.Transform.RotationDegrees + offsetAngle);
				}
			}

			position = collider.Entity.Transform.Position + center;
			bounds = new RectangleF(position.X - Radius, position.Y - Radius, Radius * 2f, Radius * 2f);
		}


		public override bool Overlaps(Shape other)
		{
			CollisionResult result;

			if (other is Box && (other as Box).IsUnrotated)
				return Collisions.RectToCircle(ref other.bounds, position, Radius);

			if (other is Circle)
				return Collisions.CircleToCircle(position, Radius, other.position, (other as Circle).Radius);

			if (other is Polygon)
				return ShapeCollisions.CircleToPolygon(this, other as Polygon, out result);

			throw new NotImplementedException($"Overlaps of Circle to {other} are not supported");
		}


		public override bool CollidesWithShape(Shape other, out CollisionResult result)
		{
			if (other is Box && (other as Box).IsUnrotated)
				return ShapeCollisions.CircleToBox(this, other as Box, out result);

			if (other is Circle)
				return ShapeCollisions.CircleToCircle(this, other as Circle, out result);

			if (other is Polygon)
				return ShapeCollisions.CircleToPolygon(this, other as Polygon, out result);

			throw new NotImplementedException($"Collisions of Circle to {other} are not supported");
		}


		public override bool CollidesWithLine(Vector2 start, Vector2 end, out RaycastHit hit)
		{
			hit = new RaycastHit();
			return ShapeCollisions.LineToCircle(start, end, this, out hit);
		}


		public override bool ContainsPoint(Vector2 point)
		{
			return ((point - position).LengthSquared() <= Radius * Radius);
		}

		#endregion


		public Vector2 GetPointAlongEdge(float angle)
		{
			return new Vector2(position.X + (Radius * Mathf.Cos(angle)), position.Y + (Radius * Mathf.Sin(angle)));
		}


		public bool ContainsPoint(float x, float y)
		{
			return ContainsPoint(new Vector2(x, y));
		}


		public bool ContainsPoint(ref Vector2 point)
		{
			return (point - position).LengthSquared() <= Radius * Radius;
		}


		public override bool PointCollidesWithShape(Vector2 point, out CollisionResult result)
		{
			return ShapeCollisions.PointToCircle(point, this, out result);
		}
	}
}