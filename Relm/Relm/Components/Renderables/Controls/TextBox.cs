using Relm.Assets.SpriteAtlases;
using Relm.Graphics.Fonts;
using Relm.Graphics.Textures;
using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;
using Relm.Core;
using Microsoft.Xna.Framework.Input;

namespace Relm.Components.Renderables.Controls
{
    public class TextBox
     : RenderableComponent, IUpdateable, IUserInterfaceRenderable
    {
        private int _width;
        private int _height;
        private readonly SpriteAtlas _atlas;
        private readonly Sprite _leftSprite;
        private readonly Sprite _centerSprite;
        private readonly Sprite _rightSprite;
        private string _text;
        private readonly IFont _font;
        private bool _hasFocus;
        private string _cursor = "|";
        private Vector2 _textOffset = new(16f, 11f);
        private Vector2 _cursorPosition = new(16f, 11f);
        private float _cursorXOffset = 16f;
        private float _elapsedTime;
        private Action<object, TextInputEventArgs, TextBox> _onTextChanged;

        public Color ShadowColor { get; set; } = Color.Black.WithOpacity(0.75f);
        public Vector2 ShadowOffset { get; set; } = Vector2.Zero * 2;

        public string Text => _text;

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

        public TextBox(IFont font, SpriteAtlas atlas, string text)
        {
            _atlas = atlas;
            _font = font;
            _text = text;
            _width = 192;
            _height = 64;
            _leftSprite = _atlas.GetSprite("TextBoxLeft");
            _centerSprite = _atlas.GetSprite("TextBoxCenter");
            _rightSprite = _atlas.GetSprite("TextBoxRight");
            Color = Color.White;
            _areBoundsDirty = true;
        }

        public TextBox(SpriteAtlas atlas, string text) : this(RelmGraphics.Instance.BitmapFont, atlas, text) { }
        public TextBox(SpriteAtlas atlas) : this(RelmGraphics.Instance.BitmapFont, atlas, "") { }
        public TextBox(IFont font, SpriteAtlas atlas) : this(font, atlas, "") { }


        public void Update()
        {
            _elapsedTime += Time.DeltaTime;

            if (_elapsedTime >= 1f)
            {
                _elapsedTime = 0f;
                _cursor = _cursor == "|" ? "" : "|";
            }

            if (Bounds.Intersects(new RectangleF(RelmInput.MousePosition, Vector2.One)))
            {
                if (RelmInput.LeftMouseButtonPressed)
                {
                    _hasFocus = !_hasFocus;

                    if (_hasFocus)
                    {
                        RelmGame.Instance.Window.TextInput += OnTextChanged;
                    }
                    else
                    {
                        RelmGame.Instance.Window.TextInput -= OnTextChanged;
                    }
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            var bounds = new RectangleF(Bounds.X, Bounds.Y, _leftSprite.SourceRect.Width, _leftSprite.SourceRect.Height);
            spriteBatch.Draw(_leftSprite, bounds, _leftSprite.SourceRect, Color.White);

            for (var x = 0;
                 x < _width - _leftSprite.SourceRect.Width - _leftSprite.SourceRect.Width;
                 x += _centerSprite.SourceRect.Width)
            {
                bounds = new RectangleF(Bounds.X + _leftSprite.SourceRect.Width + x, Bounds.Y, _centerSprite.SourceRect.Width, _centerSprite.SourceRect.Height);
                spriteBatch.Draw(_centerSprite, bounds, _centerSprite.SourceRect, Color.White);
            }

            bounds = new RectangleF(Bounds.X + _width - _rightSprite.SourceRect.Width, Bounds.Y, _rightSprite.SourceRect.Width, _rightSprite.SourceRect.Height);
            spriteBatch.Draw(_rightSprite, bounds, _rightSprite.SourceRect, Color.White);

            if (_hasFocus)
            {
                spriteBatch.DrawString(_font, _cursor, Entity.Transform.Position + _localOffset + _cursorPosition, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
                spriteBatch.DrawString(_font, _cursor, Entity.Transform.Position + _localOffset + _cursorPosition + new Vector2(0f, 8f), Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            }

            spriteBatch.DrawString(_font, _text, Entity.Transform.Position + _localOffset + _textOffset + ShadowOffset, ShadowColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            spriteBatch.DrawString(_font, _text, Entity.Transform.Position + _localOffset + _textOffset, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
        }

        #region Events

        private void OnTextChanged(object? sender, TextInputEventArgs args)
        {
            Vector2 size;

            if (args.Key == Keys.Back)
            {
                if (_text.Length > 0)
                {
                    size = _font.MeasureString(_text.Last().ToString());
                    _text = _text.Substring(0, _text.Length - 1);
                    _cursorPosition -= new Vector2(size.X, 0f);

                }
                return;
            }

            var c = args.Character;
            _text += c.ToString();
            size = _font.MeasureString(c.ToString());
            _cursorPosition += new Vector2(size.X, 0f);
        }

        #endregion

        #region Fluent Functions

        public TextBox SetSize(int width)
        {
            _width = width;
            _height = 64;
            _areBoundsDirty = true;
            return this;
        }

        public TextBox OnTextChanged(Action<object, TextInputEventArgs, TextBox> onTextChanged)
        {
            _onTextChanged = onTextChanged;
            return this;
        }

        #endregion
    }
}