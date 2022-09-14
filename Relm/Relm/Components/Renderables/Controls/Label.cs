using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Graphics.Fonts;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Controls
{
    public class Label
        : RenderableComponent, IUserInterfaceRenderable
    {
        private List<string> _lines = new();

        public IFont Font { get; set; }
        public string Text { get; set; }
        public string AppendedText { get; set; }
        public Vector2 ShadowOffset { get; set; } = new(2f);
        public Color ShadowColor { get; set; } = Color.Black.WithOpacity(0.75f);

        public int MaxWidth { get; set; } = -1;
        public int LinePadding { get; set; }

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (Font == null) return _bounds;
                    var size = Font.MeasureString(Text + AppendedText);
                    _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                        Entity.Transform.Scale, Entity.Transform.Rotation, size.X,
                        size.Y);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public Label() : this(RelmGraphics.Instance.BitmapFont, "Hello World") { }

        public Label(IFont font, string text)
        {
            Font = font;
            Text = text;
        }
        
        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            if (MaxWidth == -1)
            {
                RenderText(spriteBatch, Text + AppendedText, Entity.Transform.Position + _localOffset);
            }
            else
            {
                BreakDownIntoLines();

                var textOffset = new Vector2(0f, 0f);
                foreach (var line in _lines)
                {
                    var position = Entity.Transform.Position + _localOffset + textOffset;
                    RenderText(spriteBatch, line, position);
                    var size = Font.MeasureString(line);
                    textOffset += new Vector2(0f, size.Y + LinePadding);
                }
            }
        }

        private void RenderText(SpriteBatch spriteBatch, string text, Vector2 position)
        {
            spriteBatch.DrawString(Font, text, position + ShadowOffset, ShadowColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            spriteBatch.DrawString(Font, text, position, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
        }

        private void BreakDownIntoLines()
        {
            _lines.Clear();
            var size = Font.MeasureString(Text + AppendedText);
            if (size.X < MaxWidth)
            {
                _lines.Add(Text);
                return;
            }

            var sb = new StringBuilder();
            for (var i = 0; i < Text.Length; i++)
            {
                sb.Append(Text[i]);
                size = Font.MeasureString(sb.ToString());
                if (size.X >= MaxWidth)
                {
                    _lines.Add(sb.ToString().Trim());
                    sb.Clear();
                }
            }
            if(sb.Length > 0) _lines.Add(sb.ToString().Trim());
        }

        #region Fluent Functions

        public Label SetText(string text)
        {
            Text = text;
            _areBoundsDirty = true;
            return this;
        }

        public Label AppendText(string appendText)
        {
            AppendedText = appendText;
            _areBoundsDirty = true;
            return this;
        }

        public Label SetFont(IFont font)
        {
            Font = font;
            return this;
        }

        public Label CenterHorizontal()
        {
            var size = Font.MeasureString(Text);
            var x = (Screen.Width / 2f) - (size.X / 2);
            SetLocalOffset(x, _localOffset.Y);

            return this;
        }

        public Label CenterVertical()
        {
            var size = Font.MeasureString(Text);
            var y = (Screen.Height / 2f) - (size.Y / 2);
            SetLocalOffset(_localOffset.X, y);

            return this;
        }

        public Label SetMaxWidth(int value)
        {
            MaxWidth = value;
            return this;
        }

        #endregion
    }
}