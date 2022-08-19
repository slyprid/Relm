using Microsoft.Xna.Framework;

namespace Relm.Physics.Shapes
{
	public class Box 
        : Polygon
	{
		public float Width;
		public float Height;

        public Box(float width, float height) 
            : base(BuildBox(width, height), true)
		{
			isBox = true;
			Width = width;
			Height = height;
		}

        static Vector2[] BuildBox(float width, float height)
		{
			var halfWidth = width / 2;
			var halfHeight = height / 2;
			var verts = new Vector2[4];

			verts[0] = new Vector2(-halfWidth, -halfHeight);
			verts[1] = new Vector2(halfWidth, -halfHeight);
			verts[2] = new Vector2(halfWidth, halfHeight);
			verts[3] = new Vector2(-halfWidth, halfHeight);

			return verts;
		}

		public void UpdateBox(float width, float height)
		{
			Width = width;
			Height = height;

			var halfWidth = width / 2;
			var halfHeight = height / 2;

			Points[0] = new Vector2(-halfWidth, -halfHeight);
			Points[1] = new Vector2(halfWidth, -halfHeight);
			Points[2] = new Vector2(halfWidth, halfHeight);
			Points[3] = new Vector2(-halfWidth, halfHeight);

			for (var i = 0; i < Points.Length; i++)
				_originalPoints[i] = Points[i];
		}


		#region Shape abstract methods

		public override bool Overlaps(Shape other)
		{
			if (IsUnrotated)
			{
				if (other is Box && (other as Box).IsUnrotated)
					return bounds.Intersects(ref (other as Box).bounds);

				if (other is Circle)
					return Collisions.RectToCircle(ref bounds, other.position, (other as Circle).Radius);
			}

			return base.Overlaps(other);
		}


		public override bool CollidesWithShape(Shape other, out CollisionResult result)
		{
			if (IsUnrotated && other is Box && (other as Box).IsUnrotated)
				return ShapeCollisions.BoxToBox(this, other as Box, out result);

			return base.CollidesWithShape(other, out result);
		}


		public override bool ContainsPoint(Vector2 point)
		{
			if (IsUnrotated)
				return bounds.Contains(point);

			return base.ContainsPoint(point);
		}
		
		public override bool PointCollidesWithShape(Vector2 point, out CollisionResult result)
		{
			if (IsUnrotated)
				return ShapeCollisions.PointToBox(point, this, out result);

			return base.PointCollidesWithShape(point, out result);
		}

		#endregion
	}
}