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

        public static Texture2D GetWhitePixel(this GraphicsDevice graphicsDevice)
        {
            if (_whitePixel != null) return _whitePixel;

            _whitePixel = new Texture2D(graphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _whitePixel.SetData(new Color[1]
            {
                Color.White
            });

            return _whitePixel;
        }

        public static Texture2D Fade(this Texture2D input)
        {
            var pixels = new Color[input.Width * input.Height];
            input.GetData(pixels);

            var alpha = 0f;
            var alphaIncrement = (0.90f / input.Width);
            for (var x = 0; x < input.Width; x++)
            {
                for (var y = 0; y < input.Height; y++)
                {
                    var idx = y * input.Width + x;
                    var pixel = pixels[idx];
                    pixels[idx] = new Color(pixel, alpha);
                }
                alpha += alphaIncrement;
            }

            input.SetData(pixels);
            return input;
        }
    }
}