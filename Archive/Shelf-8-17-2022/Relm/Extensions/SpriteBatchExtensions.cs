using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Extensions
{
    public static class SpriteBatchExtensions
    {
        public static void DrawStringWithShadow(this SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color textColor, Color shadowColor)
        {
            var shadowPosition = position + (Vector2.One * 2);
            spriteBatch.DrawString(font, text, shadowPosition, shadowColor);
            spriteBatch.DrawString(font, text, position, textColor);
        }
    }
}