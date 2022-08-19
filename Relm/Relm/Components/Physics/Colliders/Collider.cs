using Microsoft.Xna.Framework;
using Relm.Math;
using Relm.Physics.Shapes;

namespace Relm.Components.Physics.Colliders
{
    public abstract class Collider 
        : Component
	{
		public Shape Shape;

		public Vector2 LocalOffset
		{
			get => _localOffset;
			set => SetLocalOffset(value);
		}

		public Vector2 AbsolutePosition => Entity.Transform.Position + _localOffset;

		public float Rotation
		{
			get
			{
				if (ShouldColliderScaleAndRotateWithTransform && Entity != null) return Entity.Transform.Rotation;

				return 0;
			}
		}

		public bool IsTrigger;

		public int PhysicsLayer = 1 << 0;

		public int CollidesWithLayers = RelmPhysics.AllLayers;

		public bool ShouldColliderScaleAndRotateWithTransform = true;

		public virtual RectangleF Bounds
		{
			get
			{
				if (_isPositionDirty || _isRotationDirty)
				{
					Shape.RecalculateBounds(this);
					_isPositionDirty = _isRotationDirty = false;
				}

				return Shape.bounds;
			}
		}

		internal RectangleF registeredPhysicsBounds;

		protected bool _colliderRequiresAutoSizing;

		protected Vector2 _localOffset;
		internal float _localOffsetLength;

		protected bool _isParentEntityAddedToScene;

		protected bool _isColliderRegistered;

		internal bool _isPositionDirty = true;
		internal bool _isRotationDirty = true;


		#region Fluent setters

		public Collider SetLocalOffset(Vector2 offset)
		{
			if (_localOffset != offset)
			{
				UnregisterColliderWithPhysicsSystem();
				_localOffset = offset;
				_localOffsetLength = _localOffset.Length();
				_isPositionDirty = true;
				RegisterColliderWithPhysicsSystem();
			}

			return this;
		}


		public Collider SetShouldColliderScaleAndRotateWithTransform(bool shouldColliderScaleAndRotateWithTransform)
		{
			ShouldColliderScaleAndRotateWithTransform = shouldColliderScaleAndRotateWithTransform;
			_isPositionDirty = _isRotationDirty = true;
			return this;
		}

		#endregion


		#region Component Lifecycle

		public override void OnAddedToEntity()
		{
			if (_colliderRequiresAutoSizing)
			{
				Assert.IsTrue(this is BoxCollider || this is CircleCollider, "Only box and circle colliders can be created automatically");

				var renderable = Entity.GetComponent<RenderableComponent>();
				Debug.WarnIf(renderable == null, "Collider has no shape and no RenderableComponent. Can't figure out how to size it.");
				if (renderable != null)
				{
					var renderableBounds = renderable.Bounds;

					var width = renderableBounds.Width / Entity.Transform.Scale.X;
					var height = renderableBounds.Height / Entity.Transform.Scale.Y;

					if (this is CircleCollider)
					{
						var circleCollider = this as CircleCollider;
						circleCollider.Radius = System.Math.Max(width, height) * 0.5f;

						LocalOffset = renderableBounds.Center - Entity.Transform.Position;
					}
					else
					{
						var boxCollider = this as BoxCollider;
						boxCollider.Width = width;
						boxCollider.Height = height;

						LocalOffset = renderableBounds.Center - Entity.Transform.Position;
					}
				}
			}

			_isParentEntityAddedToScene = true;
			RegisterColliderWithPhysicsSystem();
		}


		public override void OnRemovedFromEntity()
		{
			UnregisterColliderWithPhysicsSystem();
			_isParentEntityAddedToScene = false;
		}


		public override void OnEntityTransformChanged(Transform.Component comp)
		{
			switch (comp)
			{
				case Transform.Component.Position:
					_isPositionDirty = true;
					break;
				case Transform.Component.Scale:
					_isPositionDirty = true;
					break;
				case Transform.Component.Rotation:
					_isRotationDirty = true;
					break;
			}

			if (_isColliderRegistered)
				RelmPhysics.UpdateCollider(this);
		}


		public override void OnEnabled()
		{
			RegisterColliderWithPhysicsSystem();
			_isPositionDirty = _isRotationDirty = true;
		}


		public override void OnDisabled()
		{
			UnregisterColliderWithPhysicsSystem();
		}

		#endregion


		public virtual void RegisterColliderWithPhysicsSystem()
		{
			if (_isParentEntityAddedToScene && !_isColliderRegistered)
			{
                RelmPhysics.AddCollider(this);
				_isColliderRegistered = true;
			}
		}


		public virtual void UnregisterColliderWithPhysicsSystem()
		{
			if (_isParentEntityAddedToScene && _isColliderRegistered)
                RelmPhysics.RemoveCollider(this);
			_isColliderRegistered = false;
		}


		#region collision checks

		public bool Overlaps(Collider other)
		{
			return Shape.Overlaps(other.Shape);
		}


		public bool CollidesWith(Collider collider, out CollisionResult result)
		{
			if (Shape.CollidesWithShape(collider.Shape, out result))
			{
				result.Collider = collider;
				return true;
			}

			return false;
		}


		public bool CollidesWith(Collider collider, Vector2 motion, out CollisionResult result)
		{
			var oldPosition = Shape.position;
			Shape.position += motion;

			var didCollide = Shape.CollidesWithShape(collider.Shape, out result);
			if (didCollide)
				result.Collider = collider;

			Shape.position = oldPosition;

			return didCollide;
		}


		public bool CollidesWithAny(out CollisionResult result)
		{
			result = new CollisionResult();

			var neighbors = RelmPhysics.BoxcastBroadphaseExcludingSelf(this, CollidesWithLayers);

			foreach (var neighbor in neighbors)
			{
				// skip triggers
				if (neighbor.IsTrigger)
					continue;

				if (CollidesWith(neighbor, out result))
					return true;
			}

			return false;
		}


		public bool CollidesWithAny(ref Vector2 motion, out CollisionResult result)
		{
			result = new CollisionResult();

			var colliderBounds = Bounds;
			colliderBounds.X += motion.X;
			colliderBounds.Y += motion.Y;
			var neighbors = RelmPhysics.BoxcastBroadphaseExcludingSelf(this, ref colliderBounds, CollidesWithLayers);

			var oldPosition = Shape.position;
			Shape.position += motion;

			var didCollide = false;
			foreach (var neighbor in neighbors)
			{
				if (neighbor.IsTrigger)
					continue;

				if (CollidesWith(neighbor, out CollisionResult neighborResult))
				{
					result = neighborResult;
					motion -= neighborResult.MinimumTranslationVector;
					Shape.position -= neighborResult.MinimumTranslationVector;
					didCollide = true;
				}
			}

			Shape.position = oldPosition;

			return didCollide;
		}

		#endregion


		public override Component Clone()
		{
			var collider = MemberwiseClone() as Collider;
			collider.Entity = null;

			if (Shape != null)
				collider.Shape = Shape.Clone();

			return collider;
		}
	}
}