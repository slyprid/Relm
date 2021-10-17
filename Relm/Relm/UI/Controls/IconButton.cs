using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class IconButton
        : Control
    {
        private Action<IconButton> _onClick;
        private TextureAtlas _iconAtlas;
        private string _iconIndex;


        public TextureAtlas TextureAtlas { get; set; }
        public States.ButtonState State { get; set; }
        public Vector2 IconSize { get; set; }
        public Vector2 IconOffset { get; set; }
        public bool IsToggled { get; set; }

        public IconButton()
        {
            State = ButtonState.Normal;
            IsToggled = false;
        }

        public override void Configure()
        {
            var config = (IconButtonConfig)UserInterface.Skin.ControlConfigurations[typeof(IconButton)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            State = ButtonState.Normal;

            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                State = ButtonState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    State = ButtonState.Active;
                    _onClick?.Invoke(this);
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int)State];
            spriteBatch.Draw(region, Bounds, Color.White);

            if (IsToggled)
            {
                region = TextureAtlas[(int)ButtonState.Toggled];
                spriteBatch.Draw(region, Bounds, Color.White);
            }

            if (_iconAtlas != null)
            {
                var icon = _iconAtlas[_iconIndex];
                var iconRect = new Rectangle((int)(X + IconOffset.X), (int)(Y + IconOffset.Y), (int)IconSize.X, (int)IconSize.Y);
                spriteBatch.Draw(icon, iconRect, Color.White);
            }
        }

        #region Fluent Methods

        public IconButton HasIcon(TextureAtlas textureAtlas, string index, int iconWidth, int iconHeight)
        {
            _iconAtlas = textureAtlas;
            _iconIndex = index;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = Vector2.Zero;
            return this;
        }

        public IconButton HasIcon(TextureAtlas textureAtlas, string index, int iconWidth, int iconHeight, int offsetX, int offsetY)
        {
            _iconAtlas = textureAtlas;
            _iconIndex = index;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = new Vector2(offsetX, offsetY);
            return this;
        }
        

        public IconButton OnClick(Action<IconButton> action)
        {
            _onClick = action;
            return this;
        }

        public IconButton SetToggled(bool value)
        {
            IsToggled = value;
            return this;
        }

        #endregion
    }
}