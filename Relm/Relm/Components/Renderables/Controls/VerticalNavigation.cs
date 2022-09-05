using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Entities;
using Relm.Extensions;
using Relm.Graphics.Fonts;
using Relm.Graphics.Tweening;
using Relm.Graphics.Tweening.Interfaces;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Controls
{
    public class VerticalNavigation
        : RenderableComponent, IUpdateable, IUserInterfaceRenderable
    {
        private readonly Dictionary<string, string> _buttonText = new();
        private readonly List<Action> _actions = new();
        private readonly List<Vector2> _lineSize = new();
        private readonly List<Vector2> _cursorPositions = new();
        private readonly List<Rectangle> _itemBounds = new();
        private IFont _font;
        private readonly Texture2D _skin;
        private readonly Rectangle _cursorSrcRect = new(96, 32, 64, 64);
        private Vector2 _cursorPos = Vector2.Zero;
        private int _selectedIndex = -1;
        private readonly Transform _cursorTransform;
        private ITween<Vector2> _cursorTween;

        public Color SelectedColor { get; set; } = Color.Yellow;
        public Vector2 ShadowOffset { get; set; } = new(2f);
        public Color ShadowColor { get; set; } = Color.Black.WithOpacity(0.75f);

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (_buttonText.Count > 0)
                    {
                        var maxWidth = 0;
                        var maxHeight = 0;
                        foreach (var kvp in _buttonText)
                        {
                            var textSize = _font.MeasureString(kvp.Value);
                            maxHeight += (int)textSize.Y;
                            if (textSize.X > maxWidth) maxWidth = (int)textSize.X;
                        }

                        _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                            Entity.Transform.Scale, Entity.Transform.Rotation, maxWidth,
                            maxHeight);
                        _areBoundsDirty = false;
                    }
                }

                return _bounds;
            }
        }

        public VerticalNavigation(IFont font, Texture2D skin)
        {
            _font = font;
            _skin = skin;

            _cursorTransform = new Transform(new Entity("verticalNavigation-cursor"));
        }

        public VerticalNavigation() : this(RelmGraphics.Instance.BitmapFont, null) { }
        public VerticalNavigation(Texture2D skin) : this(RelmGraphics.Instance.BitmapFont, skin) { }
        
        public void Update()
        {
            if (_selectedIndex == -1 && _cursorPositions.Count > 0)
            {
                ChangeSelectedIndex(0);
            }

            if (RelmInput.IsKeyPressed(Keys.W) 
                || RelmInput.IsKeyPressed(Keys.Up) 
                || RelmInput.Player1Controller.DpadUpPressed 
                || RelmInput.Player1Controller.IsLeftStickUpPressed())
            {
                var value = _selectedIndex;
                value--;
                if (value < 0) value = _cursorPositions.Count - 1;
                ChangeSelectedIndex(value);
            }

            if (RelmInput.IsKeyPressed(Keys.S)
                || RelmInput.IsKeyPressed(Keys.Down)
                || RelmInput.Player1Controller.DpadDownPressed
                || RelmInput.Player1Controller.IsLeftStickDownPressed())
            {
                var value = _selectedIndex;
                value++;
                if (value >= _cursorPositions.Count) value = 0;
                ChangeSelectedIndex(value);
            }

            if (RelmInput.IsKeyPressed(Keys.Enter)
                || RelmInput.IsKeyPressed(Keys.Space)
                || RelmInput.Player1Controller.IsButtonPressed(Buttons.A)
                || RelmInput.Player1Controller.IsButtonPressed(Buttons.Start))
            {
                _actions[_selectedIndex]?.Invoke();
            }
            
            if (_itemBounds.Count == 0)
            {
                var textOffset = Vector2.Zero;
                foreach (var kvp in _buttonText)
                {
                    var textSize = _font.MeasureString(kvp.Value);
                    var x = (Entity.Transform.Position + _localOffset + textOffset).X;
                    var y = (Entity.Transform.Position + _localOffset + textOffset).Y;
                    _itemBounds.Add(new Rectangle((int)x, (int)y, (int)textSize.X, (int)textSize.Y));
                    textOffset += new Vector2(0f, textSize.Y);
                }
            }

            var idx = 0;
            foreach (var bounds in _itemBounds)
            {
                if (bounds.Intersects(new RectangleF(RelmInput.MousePosition, Vector2.One)))
                {
                    _selectedIndex = idx;
                    ChangeSelectedIndex(_selectedIndex);
                    if (RelmInput.LeftMouseButtonReleased)
                    {
                        _actions[_selectedIndex]?.Invoke();
                    }
                }

                idx++;
            }

            _cursorPos = _cursorTransform.Position;
        }
        
        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            var textOffset = Vector2.Zero;
            var index = 0;
            foreach (var kvp in _buttonText)
            {
                var color = _selectedIndex == index ? SelectedColor : Color;
                spriteBatch.DrawString(_font, kvp.Value, Entity.Transform.Position + _localOffset + textOffset + ShadowOffset, ShadowColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
                spriteBatch.DrawString(_font, kvp.Value, Entity.Transform.Position + _localOffset + textOffset, color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
                var textSize = _font.MeasureString(kvp.Value);
                textOffset += new Vector2(0f, textSize.Y);
                index++;
            }

            var destRect = new Rectangle((int)_cursorPos.X, (int)_cursorPos.Y, 64, 64);
            spriteBatch.Draw(_skin, destRect, _cursorSrcRect, Color.White);
        }

        #region Utility Methods / Functions

        private void ChangeSelectedIndex(int value)
        {
            
            _selectedIndex = value;
            _cursorTransform.Position = _cursorPositions[_selectedIndex];

            if (_cursorTween == null)
            {
                _cursorTween = _cursorTransform.TweenPositionTo(_cursorPositions[_selectedIndex] + new Vector2(24, 0));
                _cursorTween
                    .SetEaseType(EaseType.Linear)
                    .SetLoops(LoopType.PingPong, -1)
                    .SetDuration(0.5f)
                    .Start();
            }
            else
            {
                _cursorTween.Stop();
                _cursorTween = _cursorTransform.TweenPositionTo(_cursorPositions[_selectedIndex] + new Vector2(24, 0));
                _cursorTween
                    .SetEaseType(EaseType.Linear)
                    .SetLoops(LoopType.PingPong, -1)
                    .SetDuration(0.5f)
                    .Start();
            }
        }

        #endregion

        #region Fluent Functions

        public VerticalNavigation AddNavigation(string text, Action onClick)
        {
            var key = text;
            if (_buttonText.ContainsKey(key)) Assert.Fail("Attempting to add the same navigation item that exists already.");

            var textSize = _font.MeasureString(text);
            var textOffset = _lineSize.Aggregate(Vector2.Zero, (current, size) => current + new Vector2(0f, size.Y));
            _lineSize.Add(textSize);
            var pos = Entity.Transform.Position + _localOffset + textOffset + new Vector2(-96, 0);
            _cursorPositions.Add(pos);

            _buttonText.Add(key, text);
            _actions.Add(onClick);

            return this;
        }

        public VerticalNavigation SetFont(IFont font)
        {
            _font = font;
            return this;
        }

        #endregion
    }
}