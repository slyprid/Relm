using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;
using System;
using MonoGame.Extended.Input;

namespace Relm.UI.Controls
{
    public class ScrollBar
        : Control
    {
        private int _value;

        private ButtonState _leftButtonState = ButtonState.Normal;
        private ButtonState _rightButtonState = ButtonState.Normal;
        private ButtonState _upButtonState = ButtonState.Normal;
        private ButtonState _downButtonState = ButtonState.Normal;
        private ButtonState _sliderButtonState = ButtonState.Normal;

        public ScrollBarOrientation Orientation { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public int MinimumValue { get; set; }
        public int MaximumValue { get; set; }
        public Action<ScrollBar> OnValueChanged { get; set; }

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

        public ScrollBar()
        {
            Orientation = ScrollBarOrientation.Horizontal;
            MinimumValue = 0;
            MaximumValue = 100;
            Value = MaximumValue;
        }

        public override void Configure()
        {
            var config = (ScrollBarConfig)UserInterface.Skin.ControlConfigurations[typeof(ScrollBar)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (Orientation == ScrollBarOrientation.Horizontal) UpdateHorizontal();
            else UpdateVertical();

            base.Update(gameTime);
        }

        private void UpdateHorizontal()
        {
            // Pieces
            var leftButtonPiece = TextureAtlas[(int)ScrollBarPiece.LeftButtonNormal];
            var rightButtonPiece = TextureAtlas[(int)ScrollBarPiece.RightButtonNormal];
            var sliderPiece = TextureAtlas[(int)ScrollBarPiece.HorizontalSlider];

            // Position
            var valuePos = (int)((Width - (leftButtonPiece.Width + rightButtonPiece.Width + sliderPiece.Width)) * ((float)Value / (float)MaximumValue));
            var xPos = (int)Position.X + leftButtonPiece.Width + valuePos;

            // Bounds
            var leftButtonBounds = new Rectangle((int)Position.X, (int)Position.Y, leftButtonPiece.Width, leftButtonPiece.Height);
            var rightButtonBounds = new Rectangle((int)Position.X + (Width - rightButtonPiece.Width), (int)Position.Y, rightButtonPiece.Width, rightButtonPiece.Height);
            var sliderBounds = new Rectangle(xPos, (int)Position.Y, sliderPiece.Width, sliderPiece.Height);
            
            _leftButtonState = ButtonState.Normal;
            _rightButtonState = ButtonState.Normal;
            _sliderButtonState = ButtonState.Normal;

            if (leftButtonBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _leftButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _leftButtonState = ButtonState.Active;
                    Value -= 1;
                    OnValueChanged?.Invoke(this);
                }
                else if (Input.IsMouseDown(MouseButton.Left))
                {
                    _leftButtonState = ButtonState.Active;
                    Value -= 1;
                    OnValueChanged?.Invoke(this);
                }
            }

            if (rightButtonBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _rightButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _rightButtonState = ButtonState.Active;
                    Value += 1;
                    OnValueChanged?.Invoke(this);
                }
                else if (Input.IsMouseDown(MouseButton.Left))
                {
                    _rightButtonState = ButtonState.Active;
                    Value += 1;
                    OnValueChanged?.Invoke(this);
                }
            }

            if (sliderBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _sliderButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _sliderButtonState = ButtonState.Active;
                }
            }
        }

        private void UpdateVertical()
        {
            // Pieces
            var upButtonPiece = TextureAtlas[(int)ScrollBarPiece.UpButtonNormal];
            var downButtonPiece = TextureAtlas[(int)ScrollBarPiece.DownButtonNormal];
            var sliderPiece = TextureAtlas[(int)ScrollBarPiece.VerticalSlider];

            // Position
            var valuePos = (int)((Height - (upButtonPiece.Height + downButtonPiece.Height + sliderPiece.Height)) * ((float)Value / (float)MaximumValue));
            var yPos = (int)Position.Y + upButtonPiece.Height + valuePos;

            // Bounds
            var upButtonBounds = new Rectangle((int)Position.X, (int)Position.Y, upButtonPiece.Width, upButtonPiece.Height);
            var downButtonBounds = new Rectangle((int)Position.X, (int)Position.Y + (Height - downButtonPiece.Height), downButtonPiece.Width, downButtonPiece.Height);
            var sliderBounds = new Rectangle((int)Position.X, yPos, sliderPiece.Width, sliderPiece.Height);

            _upButtonState = ButtonState.Normal;
            _downButtonState = ButtonState.Normal;
            _sliderButtonState = ButtonState.Normal;

            if (upButtonBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _upButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _upButtonState = ButtonState.Active;
                    Value -= 1;
                    OnValueChanged?.Invoke(this);
                }
                else if (Input.IsMouseDown(MouseButton.Left))
                {
                    _upButtonState = ButtonState.Active;
                    Value -= 1;
                    OnValueChanged?.Invoke(this);
                }
            }

            if (downButtonBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _downButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _downButtonState = ButtonState.Active;
                    Value += 1;
                    OnValueChanged?.Invoke(this);
                }
                else if (Input.IsMouseDown(MouseButton.Left))
                {
                    _downButtonState = ButtonState.Active;
                    Value += 1;
                    OnValueChanged?.Invoke(this);
                }
            }

            if (sliderBounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                _sliderButtonState = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    _sliderButtonState = ButtonState.Active;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Orientation == ScrollBarOrientation.Horizontal) DrawHorizontal(gameTime, spriteBatch);
            else DrawVertical(gameTime, spriteBatch);
        }

        private void DrawHorizontal(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Pieces
            var leftButtonPiece = TextureAtlas[(int)ScrollBarPiece.LeftButtonNormal];
            var rightButtonPiece = TextureAtlas[(int)ScrollBarPiece.RightButtonNormal];
            var barPiece = TextureAtlas[(int)ScrollBarPiece.HorizontalBar];
            var sliderPiece = TextureAtlas[(int) ScrollBarPiece.HorizontalSlider];

            if (_leftButtonState == ButtonState.Hover) leftButtonPiece = TextureAtlas[(int)ScrollBarPiece.LeftButtonHover];
            if (_leftButtonState == ButtonState.Active) leftButtonPiece = TextureAtlas[(int)ScrollBarPiece.LeftButtonActive];
            if (_rightButtonState == ButtonState.Hover) rightButtonPiece = TextureAtlas[(int)ScrollBarPiece.RightButtonHover];
            if (_rightButtonState == ButtonState.Active) rightButtonPiece = TextureAtlas[(int)ScrollBarPiece.RightButtonActive];
            if (_sliderButtonState == ButtonState.Hover) sliderPiece = TextureAtlas[(int)ScrollBarPiece.HorizontalSliderHover];

            // Draw Bar
            var bounds = new Rectangle((int)Position.X + leftButtonPiece.Width, (int)Position.Y, Width - (leftButtonPiece.Width + rightButtonPiece.Width), barPiece.Height);
            spriteBatch.Draw(barPiece, bounds, Color.White);

            // Draw Buttons
            bounds = new Rectangle((int) Position.X, (int)Position.Y, leftButtonPiece.Width, leftButtonPiece.Height);
            spriteBatch.Draw(leftButtonPiece, bounds, Color.White);

            bounds = new Rectangle((int)Position.X + (Width - rightButtonPiece.Width), (int)Position.Y, rightButtonPiece.Width, rightButtonPiece.Height);
            spriteBatch.Draw(rightButtonPiece, bounds, Color.White);

            // Draw Slider
            var valuePos = (int)((Width - (leftButtonPiece.Width + rightButtonPiece.Width + sliderPiece.Width)) * ((float) Value / (float) MaximumValue));
            var xPos = (int) Position.X + leftButtonPiece.Width + valuePos;
            bounds = new Rectangle(xPos, (int) Position.Y, sliderPiece.Width, sliderPiece.Height);
            spriteBatch.Draw(sliderPiece, bounds, Color.White);
        }

        private void DrawVertical(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Pieces
            var upButtonPiece = TextureAtlas[(int)ScrollBarPiece.UpButtonNormal];
            var downButtonPiece = TextureAtlas[(int)ScrollBarPiece.DownButtonNormal];
            var barPiece = TextureAtlas[(int)ScrollBarPiece.VerticalBar];
            var sliderPiece = TextureAtlas[(int)ScrollBarPiece.VerticalSlider];

            if (_upButtonState == ButtonState.Hover) upButtonPiece = TextureAtlas[(int)ScrollBarPiece.UpButtonHover];
            if (_upButtonState == ButtonState.Active) upButtonPiece = TextureAtlas[(int)ScrollBarPiece.UpButtonActive];
            if (_downButtonState == ButtonState.Hover) downButtonPiece = TextureAtlas[(int)ScrollBarPiece.DownButtonHover];
            if (_downButtonState == ButtonState.Active) downButtonPiece = TextureAtlas[(int)ScrollBarPiece.DownButtonActive];
            if (_sliderButtonState == ButtonState.Hover) sliderPiece = TextureAtlas[(int)ScrollBarPiece.VerticalSliderHover];

            // Draw Bar
            var bounds = new Rectangle((int)Position.X, (int)Position.Y + upButtonPiece.Height, barPiece.Width, Height - (upButtonPiece.Height + downButtonPiece.Height));
            spriteBatch.Draw(barPiece, bounds, Color.White);

            // Draw Buttons
            bounds = new Rectangle((int)Position.X, (int)Position.Y, upButtonPiece.Width, upButtonPiece.Height);
            spriteBatch.Draw(upButtonPiece, bounds, Color.White);

            bounds = new Rectangle((int)Position.X, (int)Position.Y + (Height - downButtonPiece.Height), downButtonPiece.Width, downButtonPiece.Height);
            spriteBatch.Draw(downButtonPiece, bounds, Color.White);

            // Draw Slider
            var valuePos = (int)((Height - (upButtonPiece.Height + downButtonPiece.Height + sliderPiece.Height)) * ((float)Value / (float)MaximumValue));
            var yPos = (int)Position.Y + upButtonPiece.Height + valuePos;
            bounds = new Rectangle((int)Position.X, yPos, sliderPiece.Width, sliderPiece.Height);
            spriteBatch.Draw(sliderPiece, bounds, Color.White);
        }
        
        #region Fluent Functions

        public ScrollBar SetOrientation(ScrollBarOrientation orientation)
        {
            Orientation = orientation;
            return this;
        }

        public ScrollBar SetValues(int min, int max, int value)
        {
            MinimumValue = min;
            MaximumValue = max;
            Value = value;
            return this;
        }

        #endregion
    }
}