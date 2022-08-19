using Microsoft.Xna.Framework;
using Relm.Graphics;
using Relm.Physics.Shapes;

namespace Relm.Components.Physics.Colliders
{
	public class BoxCollider : Collider
	{
		[Inspectable]
		[Range(1, float.MaxValue, true)]
		public float Width
		{
			get => ((Box)Shape).Width;
			set => SetWidth(value);
		}

		[Inspectable]
		[Range(1, float.MaxValue, true)]
		public float Height
		{
			get => ((Box)Shape).Height;
			set => SetHeight(value);
		}


		public BoxCollider()
		{
			Shape = new Box(1, 1);
			_colliderRequiresAutoSizing = true;
		}

		public BoxCollider(float x, float y, float width, float height)
		{
			_localOffset = new Vector2(x + width / 2, y + height / 2);
			Shape = new Box(width, height);
		}

		public BoxCollider(float width, float height) : this(-width / 2, -height / 2, width, height) { }

		public BoxCollider(Rectangle rect) : this(rect.X, rect.Y, rect.Width, rect.Height) { }


		#region Fluent setters

		public BoxCollider SetSize(float width, float height)
		{
			_colliderRequiresAutoSizing = false;
			var box = Shape as Box;
			if (width != box.Width || height != box.Height)
			{
				box.UpdateBox(width, height);
				_isPositionDirty = true;
				if (Entity != null && _isParentEntityAddedToScene)
					RelmPhysics.UpdateCollider(this);
			}

			return this;
		}

		public BoxCollider SetWidth(float width)
		{
			_colliderRequiresAutoSizing = false;
			var box = Shape as Box;
			if (width != box.Width)
			{
				box.UpdateBox(width, box.Height);
				_isPositionDirty = true;
				if (Entity != null && _isParentEntityAddedToScene)
					RelmPhysics.UpdateCollider(this);
			}

			return this;
		}

		public BoxCollider SetHeight(float height)
		{
			_colliderRequiresAutoSizing = false;
			var box = Shape as Box;
			if (height != box.Height)
			{
				box.UpdateBox(box.Width, height);
				_isPositionDirty = true;
				if (Entity != null && _isParentEntityAddedToScene)
					RelmPhysics.UpdateCollider(this);
			}

			return this;
		}

		#endregion


		public override void DebugRender(SpriteBatch spriteBatch)
		{
			var poly = Shape as Polygon;
            spriteBatch.DrawHollowRect(Bounds, Debug.Colors.ColliderBounds, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPolygon(Shape.position, poly.Points, Debug.Colors.ColliderEdge, true, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Entity.Transform.Position, Debug.Colors.ColliderPosition, 4 * Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Entity.Transform.Position + Shape.center, Debug.Colors.ColliderCenter, 2 * Debug.Size.LineSizeMultiplier);
		}

		public override string ToString()
		{
			return $"[BoxCollider: bounds: {Bounds}";
		}
	}
}