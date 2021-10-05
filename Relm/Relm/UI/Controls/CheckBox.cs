using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class CheckBox
        : Control
    {
        private Action<CheckBox> _onClick;

        public TextureAtlas TextureAtlas { get; set; }
        public CheckBoxState State { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }
        public int TextPadding { get; set; }

        public bool Checked => State == CheckBoxState.Checked;

        public CheckBox()
        {
            State = CheckBoxState.Unchecked;
            TextColor = Color.Black;
            TextPadding = 8;
        }
        
        public override void Configure()
        {
            var config = (CheckBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(CheckBox)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    State = State == CheckBoxState.Unchecked ? CheckBoxState.Checked : CheckBoxState.Unchecked;
                    _onClick?.Invoke(this);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int)CheckBoxState.Unchecked];
            spriteBatch.Draw(region, Bounds, Color.White);
            
            if (State == CheckBoxState.Checked)
            {
                region = TextureAtlas[(int) CheckBoxState.Checked];
                spriteBatch.Draw(region, Bounds, Color.White);
            }

            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                var txtSize = Font.MeasureString(Text);
                var yOffset = (Height / 2) - (txtSize.Y / 2);
                spriteBatch.DrawString(Font, Text, Position + new Vector2(region.Width + TextPadding, yOffset), TextColor);
            }
        }
        
        #region Fluent Methods

        public CheckBox SetText(string text)
        {
            Text = text;
            return this;
        }

        public CheckBox SetText(string text, Color color)
        {
            Text = text;
            TextColor = color;
            return this;
        }
        
        public CheckBox OnClick(Action<CheckBox> action)
        {
            _onClick = action;    
            return this;
        }

        public CheckBox Using(string fontName)
        {
            Font = ContentLibrary.Fonts[fontName];
            return this;
        }

        #endregion
    }
}