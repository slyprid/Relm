using System;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Extensions;
using Relm.States;

namespace Relm.UI.Controls
{
    public class ProgressBar
        : Control
    {
        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(ProgressBar)];

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }

        public ProgressBar()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
            Maximum = 100;
            Value = Maximum;
        }

        public ProgressBar(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
            Maximum = 100;
            Value = Maximum;
        }

        public override void InitializeEvents()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Value < Minimum) Value = Minimum;
            if (Value > Maximum) Value = Maximum;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var partBackground = Skin[PartNames.ProgressBarBackground];
            var partFill = Skin[PartNames.ProgressBarFill];
            var fillWidth = ((float)Value / (float)Maximum) * (partFill.Width * Scale);
            var srcRect = new Rectangle(partFill.X, partFill.Y, (int)(((float)Value / (float)Maximum) * partFill.Width), partFill.Height);

            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(partBackground.Width * Scale), (int)(partBackground.Height * Scale)), partBackground, Tint.WithOpacity(Opacity));
            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(srcRect.Width * Scale), (int)(partFill.Height * Scale)), srcRect, Tint.WithOpacity(Opacity));
        }
    }
}