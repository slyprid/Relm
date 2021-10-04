using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class ProgressBar
        : Control
    {
        private int _value;
        public TextureAtlas TextureAtlas { get; set; }
        public Color FillColor { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }

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

        public ProgressBar()
        {
            MinimumValue = 0;
            MaximumValue = 100;
            Value = MaximumValue;
        }

        public override void Configure()
        {
            var config = (ProgressBarConfig)UserInterface.Skin.ControlConfigurations[typeof(ProgressBar)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var rightMost = ((float)Value / (float)MaximumValue) * (float)Width;
            var sliderPiece = TextureAtlas[ProgressBarPiece.RightFillRounded.ToString()];

            // Left Slider
            var piece = TextureAtlas[ProgressBarPiece.LeftFillRounded.ToString()];
            var bounds = new Rectangle(X, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            if(Value > MinimumValue) spriteBatch.Draw(piece, bounds, FillColor);

            // Fill
            piece = TextureAtlas[ProgressBarPiece.LeftFill.ToString()];
            var percent = (float)Value / (float)MaximumValue;
            var width = (Width - (sliderPiece.Width * 2)) * percent;
            bounds = new Rectangle(X + sliderPiece.Width, Y, (int)(width * Scale.X), (int)(piece.Height * Scale.Y));
            var srcBounds = piece.Bounds;
            srcBounds.X += sliderPiece.Width;
            srcBounds.Width -= (sliderPiece.Width * 2);
            spriteBatch.Draw(piece.Texture, bounds, srcBounds, FillColor);

            // Right Slider
            piece = TextureAtlas[ProgressBarPiece.RightFillRounded.ToString()];
            bounds = new Rectangle((int)(X + rightMost - piece.Width), Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            if(bounds.X > X + sliderPiece.Width) spriteBatch.Draw(piece, bounds, FillColor);
            
            // Left Overlay
            piece = TextureAtlas[ProgressBarPiece.LeftOverlay.ToString()];
            bounds = new Rectangle(X, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);

            // Center Overlay
            piece = TextureAtlas[ProgressBarPiece.CenterOverlay.ToString()];
            var startX = X + piece.Width;
            var endX = X + Width - piece.Width;
            for (var x = startX; x < endX; x += piece.Width)
            {
                bounds = new Rectangle(x, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White);
            }

            // Right Overlay
            piece = TextureAtlas[ProgressBarPiece.RightOverlay.ToString()];
            bounds = new Rectangle(X + (Width - piece.Width), Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);
        }

        #region Fluent Functions

        public ProgressBar WithFillColor(Color color)
        {
            FillColor = color;
            return this;
        }

        public ProgressBar SetValues(int min, int max, int value)
        {
            MinimumValue = min;
            MaximumValue = max;
            Value = value;
            return this;
        }

        #endregion
    }
}