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

        #endregion
    }
}