using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Fonts;

namespace Relm.Extensions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, string fontSetName, int fontSize, string text, Vector2 position, Color color)
        {
            var fontSet = ContentLibrary.FontSets[fontSetName];
            var spriteFont = fontSet[fontSize];
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public static void DrawString(this SpriteBatch spriteBatch, FontSet fontSet, int fontSize, string text, Vector2 position, Color color)
        {
            var spriteFont = fontSet[fontSize];
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public static void DrawStringWithShadow(this SpriteBatch spriteBatch, SpriteFont spriteFont, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(spriteFont, text, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(spriteFont, text, position, color);
        }

        public static void DrawStringWithShadow(this SpriteBatch spriteBatch, string fontSetName, int fontSize, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(fontSetName, fontSize, text, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(fontSetName, fontSize, text, position, color);
        }

        public static void DrawStringWithShadow(this SpriteBatch spriteBatch, FontSet fontSet, int fontSize, string text, Vector2 position, Color color)
        {
            spriteBatch.DrawString(fontSet, fontSize, text, position + new Vector2(2, 2), Color.Black);
            spriteBatch.DrawString(fontSet, fontSize, text, position, color);
        }
    }
}