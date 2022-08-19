using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Assets.BitmapFonts;
using Relm.Graphics.Textures;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm
{
    public class RelmGraphics
    {
        public static RelmGraphics Instance;

        public SpriteBatch SpriteBatch;
        public BitmapFont BitmapFont;
        public Sprite PixelTexture;
        
        public RelmGraphics() { }
        public RelmGraphics(BitmapFont font)
        {
            SpriteBatch = new SpriteBatch(RelmGame.GraphicsDevice);
            BitmapFont = font;

            var fontTex = BitmapFont.Textures[BitmapFont.DefaultCharacter.TexturePage];
            PixelTexture = new Sprite(fontTex, fontTex.Width - 1, fontTex.Height - 1, 1, 1);
        }


        public static Texture2D CreateSingleColorTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(RelmGame.GraphicsDevice, width, height);
            var data = new Color[width * height];
            for (var i = 0; i < data.Length; i++)
                data[i] = color;

            texture.SetData<Color>(data);
            return texture;
        }


        public void Unload()
        {
            if (PixelTexture != null)
                PixelTexture.Texture2D.Dispose();
            PixelTexture = null;

            SpriteBatch.Dispose();
            SpriteBatch = null;
        }
    }
}