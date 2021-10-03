using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;

namespace Relm.UI.Controls
{
    public class Background
        : Control
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        
        public Background()
        {
            Color = Color.White;
            Size = new Vector2(Layout.Width, Layout.Height);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null) Texture = spriteBatch.GetWhitePixel();
            spriteBatch.Draw(Texture, Bounds, Color);
        }

        #region Fluent Methods

        public Background Using(string textureName)
        {
            Texture = ContentLibrary.Textures[textureName];
            return this;
        }

        public Background SizeOf(int srcWidth, int srcHeight)
        {
            Size = new Vector2(srcWidth, srcHeight);
            return this;
        }

        public Background WithColor(Color color)
        {
            Color = color;
            return this;
        }

        #endregion
    }
}