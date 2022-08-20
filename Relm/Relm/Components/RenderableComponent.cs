using System;
using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Graphics;
using Relm.Math;

namespace Relm.Components
{
	public abstract class RenderableComponent 
        : Component, IRenderable, IComparable<RenderableComponent>
	{
		#region Properties and fields

		public virtual float Width => Bounds.Width;

		public virtual float Height => Bounds.Height;

		public virtual RectangleF Bounds
		{
			get
			{
				if (_areBoundsDirty)
				{
					_bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
						Entity.Transform.Scale, Entity.Transform.Rotation, Width, Height);
					_areBoundsDirty = false;
				}

				return _bounds;
			}
		}

		[Range(0, 1)]
		public float LayerDepth
		{
			get => _layerDepth;
			set => SetLayerDepth(value);
		}

		public int RenderLayer
		{
			get => _renderLayer;
			set => SetRenderLayer(value);
		}

		public Microsoft.Xna.Framework.Color Color = Microsoft.Xna.Framework.Color.White;

		public virtual Material Material { get; set; }

		public Vector2 LocalOffset
		{
			get => _localOffset;
			set => SetLocalOffset(value);
		}

		public bool IsVisible
		{
			get => _isVisible;
			private set
			{
				if (_isVisible != value)
				{
					_isVisible = value;

					if (_isVisible)
						OnBecameVisible();
					else
						OnBecameInvisible();
				}
			}
		}

		public bool DebugRenderEnabled = true;

		protected Vector2 _localOffset;
		protected float _layerDepth;
		protected int _renderLayer;
		protected RectangleF _bounds;
		protected bool _isVisible;
		protected bool _areBoundsDirty = true;

		#endregion

		#region Component overrides and IRenderable

		public override void OnEntityTransformChanged(Transform.Component comp)
		{
			_areBoundsDirty = true;
		}

		public abstract void Render(SpriteBatch spriteBatch, Camera camera);

		public override void DebugRender(SpriteBatch spriteBatch)
		{
			if (!DebugRenderEnabled) return;

			if (Entity.GetComponent<Collider>() == null) spriteBatch.DrawHollowRect(Bounds, Debug.Colors.RenderableBounds);

			spriteBatch.DrawPixel(Entity.Transform.Position + _localOffset, Debug.Colors.RenderableCenter, 4);
		}

		protected virtual void OnBecameVisible() { }

		protected virtual void OnBecameInvisible() { }

		public override void OnRemovedFromEntity() { }

		public virtual bool IsVisibleFromCamera(Camera camera)
		{
			IsVisible = camera.Bounds.Intersects(Bounds);
			return IsVisible;
		}

		#endregion

		#region Fluent setters

		public RenderableComponent SetMaterial(Material material)
		{
			Material = material;
			if (Entity != null && Entity.Scene != null)
				Entity.Scene.RenderableComponents.SetRenderLayerNeedsComponentSort(RenderLayer);
			return this;
		}

		public RenderableComponent SetLayerDepth(float layerDepth)
		{
			_layerDepth = Mathf.Clamp01(layerDepth);

			if (Entity != null && Entity.Scene != null)
				Entity.Scene.RenderableComponents.SetRenderLayerNeedsComponentSort(RenderLayer);
			return this;
		}

		public RenderableComponent SetRenderLayer(int renderLayer)
		{
			if (renderLayer != _renderLayer)
			{
				var oldRenderLayer = _renderLayer;
				_renderLayer = renderLayer;

				if (Entity != null && Entity.Scene != null)
					Entity.Scene.RenderableComponents.UpdateRenderableRenderLayer(this, oldRenderLayer, _renderLayer);
			}

			return this;
		}

		public RenderableComponent SetColor(Color color)
		{
			Color = color;
			return this;
		}

		public RenderableComponent SetLocalOffset(Vector2 offset)
		{
			if (_localOffset != offset)
			{
				_localOffset = offset;
				_areBoundsDirty = true;
			}

			return this;
		}

		#endregion

		#region public API

		public T GetMaterial<T>() where T : Material
		{
			return Material as T;
		}

		#endregion

		public int CompareTo(RenderableComponent other)
		{
			var res = other.RenderLayer.CompareTo(RenderLayer);
			if (res == 0)
			{
				res = other.LayerDepth.CompareTo(LayerDepth);
				if (res == 0)
				{
					if (ReferenceEquals(Material, other.Material))
						return 0;

					if (other.Material == null)
						return -1;

					return 1;
				}
			}

			return res;
		}

		public override string ToString()
		{
			return $"[RenderableComponent] {GetType()}, renderLayer: {RenderLayer}]";
		}
	}
}