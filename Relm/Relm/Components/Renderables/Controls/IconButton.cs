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
        : RenderableComponent, IUpdateable, IUserInterfaceRenderable
    {
        private int _width;
        private int _height;
        private SpriteAtlas _atlas;
        private Sprite _sprite;
        private Sprite _highlightSprite;
        private Action _onClick;
        private bool _isHover;
        private Color _hoverColor = Color.White;

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
            _isHover = false;
            if (Bounds.Intersects(new RectangleF(RelmInput.MousePosition, Vector2.One)))
            {
                _isHover = true;
                if (RelmInput.LeftMouseButtonPressed)
                {
                    _onClick?.Invoke();
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            var sprite = _sprite;
            var color = Color.White;

            if (_isHover && _highlightSprite != null)
            {
                sprite = _highlightSprite;
            }
            if (_isHover)
            {
                color = _hoverColor;
            }

            spriteBatch.Draw(sprite, Bounds, sprite.SourceRect, color);
        }

        #region Fluent Functions

        public IconButton OnClick(Action onClick)
        {
            _onClick = onClick;
            return this;
        }

        public IconButton HasHover(string iconName)
        {
            _highlightSprite = _atlas.GetSprite(iconName);
            return this;
        }

        public IconButton HasHoverColor(Color color)
        {
            _hoverColor = color;
            return this;
        }

        public IconButton ChangeIcon(string name)
        {
            _sprite = _atlas.GetSprite(name);
            _width = _sprite.SourceRect.Width;
            _height = _sprite.SourceRect.Height;
            _areBoundsDirty = true;
            return this;
        }

        #endregion
    }
}