using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Extensions
{
    public static class TextureExtensions
    {
        public static Texture2D _whitePixel;

        private static void LoadWhitePixel(SpriteBatch spriteBatch)
        {
            if (_whitePixel != null) return;

            _whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _whitePixel.SetData(new Color[1]
            {
                Color.White
            });
        }
    }
}