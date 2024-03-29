﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Sprites;

namespace Relm.UserInterface
{
    public class Label
        : BaseControl
    {
        private readonly UserInterfaceSkin _skin;
        private SpriteFont _font;

        public SpriteFont Font
        {
            get => _font ?? _skin.Font;
            set => _font = value;
        }

        public string Text { get; set; }
        public Color Color { get; set; }
        public Color ShadowColor { get; set; }
        public Vector2 ShadowOffset { get; set; }

        public Label(UserInterfaceSkin skin)
            : base(skin)
        {
            _skin = skin;
            Color = Color.White;
            ShadowColor = Color.Black.WithOpacity(192);
            ShadowOffset = new Vector2(2, 2);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var position = CalculatePosition(this);

            spriteBatch.DrawString(Font, Text, position + ShadowOffset, ShadowColor);
            spriteBatch.DrawString(Font, Text, position, Color);
        }

        #region Fluent Functions

        public Label WithText(string text)
        {
            Text = text;
            return this;
        }

        public Label WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public override OldSprite WithPosition(ScreenPosition position)
        {
            switch (position)
            {
                case ScreenPosition.TopLeft:
                    Position = Layout.TopLeft;
                    break;
                case ScreenPosition.TopCenter:
                    Position = Layout.TopCenter;
                    break;
                case ScreenPosition.TopRight:
                    Position = Layout.TopRight;
                    break;
                case ScreenPosition.CenterLeft:
                    Position = Layout.CenterLeft;
                    break;
                case ScreenPosition.CenterRight:
                    Position = Layout.CenterRight;
                    break;
                case ScreenPosition.BottomLeft:
                    Position = Layout.BottomLeft;
                    break;
                case ScreenPosition.BottomCenter:
                    Position = Layout.BottomCenter;
                    break;
                case ScreenPosition.BottomRight:
                    Position = Layout.BottomRight;
                    break;
                case ScreenPosition.LeftAlignedCentered:
                    Position = Layout.Centered(Width, Height);
                    break;
                default:
                case ScreenPosition.Centered:
                    var textSize = Font.MeasureString(Text);
                    if (Parent != null)
                    {
                        Position = new Vector2(Parent.Width / 2f, Parent.Height / 2f)  - new Vector2(textSize.X / 2f, textSize.Y / 2f);
                    }
                    else
                    {
                        Position = Layout.Centered((int)textSize.X, (int)textSize.Y);
                    }
                    
                    break;
            }

            return this;
        }

        public Label WithFont(SpriteFont font)
        {
            Font = font;
            return this;
        }

        #endregion
    }
}