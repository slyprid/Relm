using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class Slider
        : Control
    {
        private bool _isDragging;
        private int _startMouseX;
        private int _value;
        public TextureAtlas TextureAtlas { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public int FontSize { get; set; }
        public bool HasTextShadow { get; set; }
        public Vector2 TextShadowOffset { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public bool ShowValue { get; set; }

        public int Value
        {
            get => _value;
            set
            {
                if (value > MaximumValue) value = MaximumValue;
                if (value < MinimumValue) value = MinimumValue;
                _value = value;
            }
        }

        public Rectangle SliderBounds
        {
            get
            {
                var region = TextureAtlas[(int)SliderPieces.Slider];
                var percent = (float)Value / (float)MaximumValue;
                var xOffset = Width * percent;
                return new Rectangle((int)Position.X - (region.Width / 2) + (int)xOffset, (int)Position.Y + 1, region.Width, region.Height);
            }
        }

        public Slider()
        {
            TextColor = Color.Black;
            FontSize = 16;
            HasTextShadow = true;
            TextShadowOffset = new Vector2(2f, 2f);
            MinimumValue = 0;
            MaximumValue = 100;
            Value = MaximumValue;
            ShowValue = true;
        }

        public override void Configure()
        {
            var config = (SliderConfig)UserInterface.Skin.ControlConfigurations[typeof(Slider)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (SliderBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                if (Input.IsMouseDown(MouseButton.Left))
                {
                    if (!_isDragging)
                    {
                        _isDragging = true;
                        _startMouseX = MouseState.X;
                    }
                }
            }

            if (_isDragging)
            {
                var deltaX = MouseState.X - _startMouseX;
                Value += deltaX;
                _startMouseX = MouseState.X;
            }

            if (Input.IsMouseUp(MouseButton.Left) && _isDragging)
            {
                _isDragging = false;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int)SliderPieces.Background];
            spriteBatch.Draw(region, Bounds, Color.White);
            
            if (!string.IsNullOrEmpty(Text))
            {
                var fontSet = UserInterface.Skin.FontSet;
                var font = fontSet[FontSize];
                var text = $"{Text}";
                if (ShowValue)
                {
                    text = $"{Text} - {Value}";
                }
                var textSize = font.MeasureString(text);
                var textPosition = Position + new Vector2((Width / 2) - (textSize.X / 2), (Height / 2) - (textSize.Y / 2));
                if (HasTextShadow)
                {
                    spriteBatch.DrawString(font, text, textPosition + TextShadowOffset, Color.Black);
                }
                spriteBatch.DrawString(font, text, textPosition, TextColor);
            }

            region = TextureAtlas[(int)SliderPieces.Slider];
            spriteBatch.Draw(region, SliderBounds, Color.White);
        }

        #region Fluent Methods

        public Slider SetText(string text)
        {
            Text = text;
            return this;
        }

        public Slider SetText(string text, Color color)
        {
            Text = text;
            TextColor = color;
            return this;
        }

        public Slider SetText(string text, int fontSize, Color color)
        {
            Text = text;
            TextColor = color;
            FontSize = fontSize;
            return this;
        }

        public Slider SetValues(int min, int max, int value)
        {
            MinimumValue = min;
            MaximumValue = max;
            Value = value;
            return this;
        }

        #endregion
    }
}