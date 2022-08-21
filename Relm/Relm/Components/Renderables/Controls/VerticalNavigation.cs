using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Fonts;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Controls
{
    public class VerticalNavigation
        : RenderableComponent
    {
        private readonly Dictionary<string, string> _buttonText = new();
        private readonly Dictionary<string, Action> _actions = new();
        private IFont _font;

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

        public VerticalNavigation()
        {
            _font = RelmGraphics.Instance.BitmapFont;
            //_bounds = new RectangleF(0f, 0f, Screen.Width, Screen.Height);
            //_areBoundsDirty = false;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
        }

        public void Update()
        {
            
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            var textOffset = Vector2.Zero;
            foreach (var kvp in _buttonText)
            {
                spriteBatch.DrawString(_font, kvp.Value, Entity.Transform.Position + _localOffset + textOffset, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
                var textSize = _font.MeasureString(kvp.Value);
                textOffset += new Vector2(0f, textSize.Y);
            }
        }

        #region Fluent Functions

        public VerticalNavigation AddNavigation(string text, Action onClick)
        {
            var key = text;
            if (_buttonText.ContainsKey(key) || _actions.ContainsKey(key)) Assert.Fail("Attempting to add the same navigation item that exists already.");

            _buttonText.Add(key, text);
            _actions.Add(key, onClick);

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