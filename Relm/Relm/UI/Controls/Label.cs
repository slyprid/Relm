using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Extensions;
using Relm.Sprites;
using Relm.States;

namespace Relm.UI.Controls
{
    public class Label
        : Control
    {
        public override string Name { get; protected set; }

        private CenteringDirection _centeringDirection = CenteringDirection.None;

        public string Text { get; set; }
        public Color ForegroundColor { get; set; }

        public bool HasShadow { get; set; }

        public string FontName { get; set; }

        public SpriteFont Font => GameState.Fonts[FontName];


        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(Label)];
        
        public Label()
        {
            Name = Guid.NewGuid().ToString();
            ForegroundColor = Color.Black;
        }

        public Label(string name)
        {
            Name = name;
            ForegroundColor = Color.Black;
        }

        public override void InitializeEvents() { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_centeringDirection != CenteringDirection.None)
            {
                IsCentered(_centeringDirection);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            if (string.IsNullOrEmpty(FontName)) throw new Exception("No font set on label");

            if (HasShadow)
            {
                spriteBatch.DrawString(Font, Text, Position.ToVector2() + new Vector2(2f, 2f), Color.Black.WithOpacity(0.75f));
            }

            spriteBatch.DrawString(Font, Text, Position.ToVector2(), ForegroundColor);
        }

        public override Sprite IsCentered(CenteringDirection direction)
        {
            _centeringDirection = direction;

            var size = Font.MeasureString(Text);
            var x = Position.X;
            var y = Position.Y;
            var width = (int)size.X;
            var height = (int)size.Y;
            switch (direction)
            {
                case CenteringDirection.Both:
                    x = (PositionConstants.CenterScreen.X) - (width / 2);
                    y = (PositionConstants.CenterScreen.Y) - (height / 2);
                    break;
                case CenteringDirection.Horizontal:
                    x = (PositionConstants.CenterScreen.X) - (width / 2);
                    break;
                case CenteringDirection.Vertical:
                    y = (PositionConstants.CenterScreen.Y) - (height / 2);
                    break;
            }

            Position = new Point(x, y);

            return this;
        }
    }
}