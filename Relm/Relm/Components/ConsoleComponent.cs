using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Relm.Components
{
    public class ConsoleComponent
        : SimpleDrawableGameComponent
    {
        private readonly List<Tuple<string, Color>> _lines = new List<Tuple<string, Color>>();

        public SpriteFont Font { get; set; }
        public RelmGame Game { get; set; }

        public ConsoleComponent()
        {
            DrawOrder = Int32.MaxValue;
        }

        public override void Update(GameTime gameTime)
        {
            if (Font == null) return;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Font == null) return;

            var spriteBatch = Game.SpriteBatch;

            spriteBatch.Begin();

            var index = 0;
            foreach (var line in _lines)
            {
                var text = line.Item1;
                var color = line.Item2;
                var size = Font.MeasureString(text);
                spriteBatch.DrawString(Font, text, new Vector2(0, (index * size.Y)), color);
                index++;
            }

            spriteBatch.End();

            _lines.Clear();
        }

        public void WriteLine(string message)
        {
            _lines.Add(new Tuple<string, Color>(message, Color.White));
        }

        public void WriteLine(string message, Color color)
        {
            _lines.Add(new Tuple<string, Color>(message, color));
        }
    }
}