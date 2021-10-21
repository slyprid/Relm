using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;
using Relm.UI.Configuration;
using ButtonState = Relm.UI.States.ButtonState;

namespace Relm.UI.Controls
{
    public class Button
        : Control
    {
        private Action<Button> _onClick;
        private bool _locked;

        public TextureAtlas TextureAtlas { get; set; }
        public States.ButtonState State { get; set; }
        public Texture2D Icon { get; set; }
        public Vector2 IconSize { get; set; }
        public Vector2 IconOffset { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public int FontSize { get; set; }
        public bool HasTextShadow { get; set; }
        public Vector2 TextShadowOffset { get; set; }
        public bool IsToggled { get; set; }

        public Button()
        {
            State = ButtonState.Normal;
            TextColor = Color.Black;
            FontSize = 16;
            HasTextShadow = true;
            TextShadowOffset = new Vector2(2f, 2f);
            IsToggled = false;
        }
        
        public override void Configure()
        {
            var config = (ButtonConfig)UserInterface.Skin.ControlConfigurations[typeof(Button)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
            State = ButtonState.Normal;

            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                State = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    if (!_locked)
                    {
                        State = ButtonState.Active;
                        _onClick?.Invoke(this);
                        _locked = true;
                    }
                }
                else if (Input.IsMouseUp(MouseButton.Left))
                {
                    _locked = false;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int) State];
            spriteBatch.Draw(region, Bounds, Color.White.WithOpacity(Opacity));

            if (IsToggled)
            {
                region = TextureAtlas[(int)ButtonState.Toggled];
                spriteBatch.Draw(region, Bounds, Color.White.WithOpacity(Opacity));
            }

            if (Icon != null)
            {
                var iconRect = new Rectangle((int) (X + IconOffset.X), (int) (Y + IconOffset.Y), (int) IconSize.X, (int) IconSize.Y);
                spriteBatch.Draw(Icon, iconRect, Color.White.WithOpacity(Opacity));
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var fontSet = UserInterface.Skin.FontSet;
                var font = fontSet[FontSize];
                var textSize = font.MeasureString(Text);
                var textPosition = Position + new Vector2((Width / 2) - (textSize.X / 2), (Height / 2) - (textSize.Y / 2));
                if (HasTextShadow)
                {
                    spriteBatch.DrawString(font, Text, textPosition + TextShadowOffset, Color.Black.WithOpacity(Opacity));
                }
                spriteBatch.DrawString(font, Text, textPosition, TextColor.WithOpacity(Opacity));
            }
        }

        #region Fluent Methods
        
        public Button HasIcon(Texture2D texture)
        {
            Icon = texture;
            IconSize = new Vector2(texture.Width, texture.Height);
            IconOffset = Vector2.Zero;
            return this;
        }

        public Button HasIcon(Texture2D texture, int iconWidth, int iconHeight)
        {
            Icon = texture;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = Vector2.Zero;
            return this;
        }

        public Button HasIcon(Texture2D texture, int iconWidth, int iconHeight, int offsetX, int offsetY)
        {
            Icon = texture;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = new Vector2(offsetX, offsetY);
            return this;
        }

        public Button SetText(string text)
        {
            Text = text;
            return this;
        }

        public Button SetText(string text, Color color)
        {
            Text = text;
            TextColor = color;
            return this;
        }

        public Button SetText(string text, int fontSize, Color color)
        {
            Text = text;
            TextColor = color;
            FontSize = fontSize;
            return this;
        }

        public Button OnClick(Action<Button> action)
        {
            _onClick = action;
            return this;
        }

        public Button SetToggled(bool value)
        {
            IsToggled = value;
            return this;
        }

        #endregion
    }
}