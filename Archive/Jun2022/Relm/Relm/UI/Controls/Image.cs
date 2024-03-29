﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;

namespace Relm.UI.Controls
{
    public class Image
        : Control
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }

        public Image()
        {
            Color = Color.White;
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            spriteBatch.Draw(Texture, Bounds, Color.WithOpacity(Opacity));
        }

        #region Fluent Methods

        public Image Using(string textureName)
        {
            Texture = ContentLibrary.Textures[textureName];
            Size = new Vector2(Texture.Width, Texture.Height);
            return this;
        }

        public Image SizeOf(int srcWidth, int srcHeight)
        {
            Size = new Vector2(srcWidth, srcHeight);
            return this;
        }

        public Image WithColor(Color color)
        {
            Color = color;
            return this;
        }

        #endregion
    }
}