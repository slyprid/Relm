using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Extensions
{
    public static class TextureExtensions
    {
        public static Texture2D TextureFromStreamPreMultiplied(this Stream stream)
        {
            var texture = Texture2D.FromStream(RelmGame.GraphicsDevice, stream);

            var pixels = new byte[texture.Width * texture.Height * 4];
            texture.GetData(pixels);
            PremultiplyAlpha(pixels);
            texture.SetData(pixels);

            return texture;
        }

        static unsafe void PremultiplyAlpha(byte[] pixels)
        {
            fixed (byte* b = &pixels[0])
            {
                for (var i = 0; i < pixels.Length; i += 4)
                {
                    if (b[i + 3] != 255)
                    {
                        var alpha = b[i + 3] / 255f;
                        b[i + 0] = (byte)(b[i + 0] * alpha);
                        b[i + 1] = (byte)(b[i + 1] * alpha);
                        b[i + 2] = (byte)(b[i + 2] * alpha);
                    }
                }
            }
        }
	}
}