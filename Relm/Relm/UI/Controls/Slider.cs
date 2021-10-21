using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class Slider
        : Control
    {
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
                var xOffset = (Width * Scale.X) * percent;
                return new Rectangle((int)Position.X - (int)((region.Width) / 2) + (int)xOffset, (int)Position.Y + 1, (int)(region.Width), (int)(region.Height));
            }
        }

        public int SliderPosition => SliderBounds.X;

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
            if (!IsEnabled) return;
            var state = Input.GetMouseState();

            if (Bounds.ExtendWidthBoth(16).Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                if (state.IsButtonDown(MouseButton.Left))
                {
                    var region = TextureAtlas[(int)SliderPieces.Slider];
                    var max = (int)Position.X + (Width * Scale.X) - (int)(region.Width / 2f);
                    var min = (int)Position.X - (int)(region.Width / 2f);
                    var range = (max - min);
                    var position = MouseState.X;
                    var valuePosition = range - (max - position);
                    var percent = (float)valuePosition / (float)range;
                    Value = (int)(percent * MaximumValue);
                    if (position < min) Value = MinimumValue;
                    if (position > max) Value = MaximumValue;
                }
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
                var textPosition = Position + new Vector2(((Width * Scale.X) / 2) - (textSize.X / 2), ((Height * Scale.Y) / 2) - (textSize.Y / 2));
                if (HasTextShadow)
                {
                    spriteBatch.DrawString(font, text, textPosition + TextShadowOffset, Color.Black.WithOpacity(Opacity));
                }
                spriteBatch.DrawString(font, text, textPosition, TextColor.WithOpacity(Opacity));
            }

            region = TextureAtlas[(int)SliderPieces.Slider];
            spriteBatch.Draw(region, SliderBounds, Color.White.WithOpacity(Opacity));
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