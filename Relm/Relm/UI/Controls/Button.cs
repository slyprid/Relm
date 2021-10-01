using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using ButtonState = Relm.UI.States.ButtonState;

namespace Relm.UI.Controls
{
    public class Button
        : Control
    {
        public TextureAtlas TextureAtlas { get; set; }
        public States.ButtonState State { get; set; }
        public Texture2D Icon { get; set; }
        public Vector2 IconSize { get; set; }
        public Vector2 IconOffset { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }
        

        public Button()
        {
            State = ButtonState.Normal;
            TextColor = Color.Black;
        }
        
        public override void Configure()
        {
            var config = (ButtonConfig)UserInterface.Skin.ControlConfigurations[typeof(Button)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            State = ButtonState.Normal;

            if (!Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1))) return;

            State = ButtonState.Hover;
            if (MouseState.WasButtonJustDown(MouseButton.Left))
            {
                State = ButtonState.Active;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int) State];
            spriteBatch.Draw(region, Bounds, Color.White);

            if (Icon != null)
            {
                var iconRect = new Rectangle((int) (X + IconOffset.X), (int) (Y + IconOffset.Y), (int) IconSize.X, (int) IconSize.Y);
                spriteBatch.Draw(Icon, iconRect, Color.White);
            }

            if (!string.IsNullOrEmpty(Text))
            {
                var font = UserInterface.Skin.Font;
                var textSize = font.MeasureString(Text);
                var textPosition = Position + new Vector2((Width / 2) - (textSize.X / 2), (Height / 2) - (textSize.Y / 2));
                spriteBatch.DrawString(font, Text, textPosition, TextColor);
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

        public Button OnClick(Action<object, MouseEventArgs> action, UserInterfaceScreen screen)
        {
            void OnClickAction(object sender, MouseEventArgs args)
            {
                if (!Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1))) return;
                action.Invoke(sender, args);
            }

            Input.OnMouseClicked(MouseButton.Left, OnClickAction, screen);
            return this;
        }

        #endregion
    }
}