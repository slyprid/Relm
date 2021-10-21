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
    public class CheckBox
        : Control
    {
        private Action<CheckBox> _onClick;
        private bool _locked;

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
            _locked = false;
        }
        
        public override void Configure()
        {
            var config = (CheckBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(CheckBox)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
            base.Update(gameTime);
            
            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    if (!_locked)
                    {
                        State = State == CheckBoxState.Unchecked ? CheckBoxState.Checked : CheckBoxState.Unchecked;
                        _onClick?.Invoke(this);
                        _locked = true;
                    }
                }
                else if (Input.IsMouseUp(MouseButton.Left))
                {
                    _locked = false;
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int)CheckBoxState.Unchecked];
            spriteBatch.Draw(region, Bounds, Color.White.WithOpacity(Opacity));
            
            if (State == CheckBoxState.Checked)
            {
                region = TextureAtlas[(int) CheckBoxState.Checked];
                spriteBatch.Draw(region, Bounds, Color.White.WithOpacity(Opacity));
            }

            region = TextureAtlas[(int)CheckBoxState.Unchecked];
            if (Font != null && !string.IsNullOrEmpty(Text))
            {
                var txtSize = Font.MeasureString(Text);
                var yOffset = ((Height * Scale.Y) / 2) - (txtSize.Y / 2);
                spriteBatch.DrawString(Font, Text, Position + new Vector2((region.Width * Scale.X) + TextPadding, yOffset), TextColor.WithOpacity(Opacity));
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

        public CheckBox Using(string fontSetName, int size)
        {
            var fontSet = ContentLibrary.FontSets[fontSetName];
            var font = fontSet[size];
            Font = font;
            return this;
        }

        #endregion
    }
}