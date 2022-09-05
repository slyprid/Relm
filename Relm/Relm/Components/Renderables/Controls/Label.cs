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
        public IFont Font { get; set; }
        public string Text { get; set; }
        public Vector2 ShadowOffset { get; set; } = new(2f);
        public Color ShadowColor { get; set; } = Color.Black.WithOpacity(0.75f);

        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    if (Font == null) return _bounds;
                    var size = Font.MeasureString(Text);
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
            spriteBatch.DrawString(Font, Text, Entity.Transform.Position + _localOffset + ShadowOffset, ShadowColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            spriteBatch.DrawString(Font, Text, Entity.Transform.Position + _localOffset, Color, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
        }

        #region Fluent Functions

        public Label SetText(string text)
        {
            Text = text;
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

        #endregion
    }
}