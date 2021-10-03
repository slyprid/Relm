using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Extensions
{
    public static class TextureExtensions
    {
        private static Texture2D _whitePixel;

        public static Texture2D GetWhitePixel(this SpriteBatch spriteBatch)
        {
            if (_whitePixel != null) return _whitePixel;

            _whitePixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _whitePixel.SetData(new Color[1]
            {
                Color.White
            });

            return _whitePixel;
        }
    }
}