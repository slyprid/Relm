﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Sprites
{
	public class PrototypeSpriteRenderer : SpriteRenderer
	{
		public override float Width => _width;
		public override float Height => _height;

		public override RectangleF Bounds
		{
			get
			{
				if (_areBoundsDirty)
				{
					_bounds.CalculateBounds(Entity.Transform.Position, _localOffset, _origin, Entity.Transform.Scale,
						Entity.Transform.Rotation, _width, _height);
					_areBoundsDirty = false;
				}

				return _bounds;
			}
		}

		public float SkewTopX;
		public float SkewBottomX;
		public float SkewLeftY;
		public float SkewRightY;

		float _width, _height;


		public PrototypeSpriteRenderer() : this(50, 50)
		{ }

		public PrototypeSpriteRenderer(float width, float height) : base(RelmGraphics.Instance.PixelTexture)
		{
			_width = width;
			_height = height;
		}

        public PrototypeSpriteRenderer(Texture2D texture, float width, float height) : base(texture)
        {
            _width = width;
            _height = height;
        }

        /// <summary>
        /// sets the width of the sprite
        /// </summary>
        /// <returns>The width.</returns>
        /// <param name="width">Width.</param>
        public PrototypeSpriteRenderer SetWidth(float width)
		{
			_width = width;
			return this;
		}

		/// <summary>
		/// sets the height of the sprite
		/// </summary>
		/// <returns>The height.</returns>
		/// <param name="height">Height.</param>
		public PrototypeSpriteRenderer SetHeight(float height)
		{
			_height = height;
			return this;
		}

		/// <summary>
		/// sets the skew values for the sprite
		/// </summary>
		/// <returns>The skew.</returns>
		/// <param name="skewTopX">Skew top x.</param>
		/// <param name="skewBottomX">Skew bottom x.</param>
		/// <param name="skewLeftY">Skew left y.</param>
		/// <param name="skewRightY">Skew right y.</param>
		public PrototypeSpriteRenderer SetSkew(float skewTopX, float skewBottomX, float skewLeftY, float skewRightY)
		{
			SkewTopX = skewTopX;
			SkewBottomX = skewBottomX;
			SkewLeftY = skewLeftY;
			SkewRightY = skewRightY;
			return this;
		}

		public override void OnAddedToEntity()
		{
			OriginNormalized = Vector2Extensions.HalfVector();
		}

		public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            //var origin = Origin;
            var origin = Vector2.Zero;

            var pos = (Entity.Transform.Position - (origin * Entity.Transform.Scale) + LocalOffset);
			var size = new Point((int)(_width * Entity.Transform.Scale.X), (int)(_height * Entity.Transform.Scale.Y));
			var destRect = new Rectangle((int)pos.X, (int)pos.Y, size.X, size.Y);
			spriteBatch.Draw(_sprite, destRect, _sprite.SourceRect, Color, Entity.Transform.Rotation,
				Microsoft.Xna.Framework.Graphics.SpriteEffects.None, LayerDepth, SkewTopX, SkewBottomX, SkewLeftY, SkewRightY);
		}
	}
}