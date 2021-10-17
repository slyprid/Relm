using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Relm.UI.Controls
{
    public class Label
        : Control
    {
        private Action<Label> _onUpdate;

        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }

        public override int Width
        {
            get
            {
                if (string.IsNullOrEmpty(Text) || Font == null) return base.Width;
                var size = Font.MeasureString(Text);
                return (int)size.X;
            }
        }

        public override int Height
        {
            get
            {
                if (string.IsNullOrEmpty(Text) || Font == null) return base.Height;
                var size = Font.MeasureString(Text);
                return (int)size.Y;
            }
        }

        public Label()
        {
            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            _onUpdate?.Invoke(this);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (string.IsNullOrEmpty(Text)) return;
            spriteBatch.DrawString(Font, Text, Position, Color);
        }

        #region Fluent Methods

        public Label Using(string fontName)
        {
            Font = ContentLibrary.Fonts[fontName];
            return this;
        }

        public Label Using(string fontSetName, int size)
        {
            var fontSet = ContentLibrary.FontSets[fontSetName];
            Font = fontSet[size];
            return this;
        }

        public Label WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Label OnUpdate(Action<Label> action)
        {
            _onUpdate = action;
            return this;
        }

        public Label SetText(string text)
        {
            Text = text;
            return this;
        }

        #endregion
    }
}