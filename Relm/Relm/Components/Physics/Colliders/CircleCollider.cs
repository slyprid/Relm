using Relm.Graphics;
using Relm.Physics.Shapes;

namespace Relm.Components.Physics.Colliders
{
	public class CircleCollider : Collider
	{
		[Inspectable]
		public float Radius
		{
			get => ((Circle)Shape).Radius;
			set => SetRadius(value);
		}


		public CircleCollider()
		{
			Shape = new Circle(1);
			_colliderRequiresAutoSizing = true;
		}


		public CircleCollider(float radius)
		{
			Shape = new Circle(radius);
		}


		#region Fluent setters

		public CircleCollider SetRadius(float radius)
		{
			_colliderRequiresAutoSizing = false;
			var circle = Shape as Circle;
			if (radius != circle.Radius)
			{
				circle.Radius = radius;
				circle._originalRadius = radius;
				_isPositionDirty = true;

				if (Entity != null && _isParentEntityAddedToScene)
					RelmPhysics.UpdateCollider(this);
			}

			return this;
		}

		#endregion


		public override void DebugRender(SpriteBatch spriteBatch)
		{
            spriteBatch.DrawHollowRect(Bounds, Debug.Colors.ColliderBounds, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawCircle(Shape.position, ((Circle)Shape).Radius, Debug.Colors.ColliderEdge, Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Entity.Transform.Position, Debug.Colors.ColliderPosition, 4 * Debug.Size.LineSizeMultiplier);
            spriteBatch.DrawPixel(Shape.position, Debug.Colors.ColliderCenter, 2 * Debug.Size.LineSizeMultiplier);
		}

		public override string ToString()
		{
			return $"[CircleCollider: bounds: {Bounds}, radius: {((Circle)Shape).Radius}";
		}
	}
}