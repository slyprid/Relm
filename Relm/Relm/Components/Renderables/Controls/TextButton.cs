using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Assets.SpriteAtlases;
using Relm.Extensions;
using Relm.Graphics.Fonts;
using Relm.Graphics.Textures;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Controls
{
    public class TextButton
        : RenderableComponent, IUpdateable
    {
        private int _width;
        private int _height;
        private SpriteAtlas _atlas;
        private Sprite _sprite;
        private Sprite _highlightSprite;
        private Action _onClick;
        private string _text;
        private IFont _font;
        private bool _isHover;

        public Color ShadowColor { get; set; } = Color.Black.WithOpacity(0.75f);
        public Vector2 ShadowOffset { get; set; } = Vector2.Zero * 2;

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    var size = _font.MeasureString(_text);
                    if (_width < size.X + 48)
                    {
                        _width = (int)(size.X + 48);
                    }

                    _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                        Entity.Transform.Scale, Entity.Transform.Rotation, _width,
                        _height);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public TextButton(IFont font, SpriteAtlas atlas, string text)
        {
            _atlas = atlas;
            _font = font;
            _text = text;
            _width = 128;
            _height = 64;
            _sprite = _atlas.GetSprite("WideButton");
            _highlightSprite = _atlas.GetSprite("WideButtonHighlight");
            Color = Color.Black;
            _areBoundsDirty = true;
        }

        public TextButton(SpriteAtlas atlas, string text) : this(RelmGraphics.Instance.BitmapFont, atlas, text) { }
        

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
            var sprite = _isHover ? _highlightSprite : _sprite;
            spriteBatch.Draw(sprite, Bounds, sprite.SourceRect, Color.White);

            var size = _font.MeasureString(_text);
            var textOffset = new Vector2(0f, 10f);
            var ePosition = Entity.Transform.Position + _localOffset;
            var pos = new Vector2((_width / 2f) - (size.X / 2), (_height / 2f) - (size.Y / 2)) + textOffset;
            spriteBatch.DrawString(_font, _text, Entity.Transform.Position + _localOffset + pos + ShadowOffset, ShadowColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            spriteBatch.DrawString(_font, _text, Entity.Transform.Position + _localOffset + pos, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
        }

        #region Fluent Functions

        public TextButton OnClick(Action onClick)
        {
            _onClick = onClick;
            return this;
        }

        #endregion
    }
}