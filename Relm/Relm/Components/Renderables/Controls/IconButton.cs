using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Assets.SpriteAtlases;
using Relm.Graphics;
using Relm.Graphics.Textures;
using Relm.Math;
using Relm.UI;
using System;

namespace Relm.Components.Renderables.Controls
{
    public class IconButton
        : RenderableComponent, IUpdateable
    {
        private int _width;
        private int _height;
        private SpriteAtlas _atlas;
        private Sprite _sprite;
        private Action _onClick;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                        Entity.Transform.Scale, Entity.Transform.Rotation, _width,
                        _height);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public IconButton(SpriteAtlas atlas, string iconName)
        {
            _atlas = atlas;
            _sprite = _atlas.GetSprite(iconName);
            _width = _sprite.SourceRect.Width;
            _height = _sprite.SourceRect.Height;
            _areBoundsDirty = true;
        }

        public void Update()
        {
            if (Bounds.Intersects(new RectangleF(RelmInput.MousePosition, Vector2.One)))
            {
                if (RelmInput.LeftMouseButtonPressed)
                {
                    _onClick?.Invoke();
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(_sprite, Bounds, _sprite.SourceRect, Color.White);
        }

        #region Fluent Functions

        public IconButton OnClick(Action onClick)
        {
            _onClick = onClick;
            return this;
        }

        #endregion
    }
}