using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Fonts;

namespace Relm.Extensions
{
    public static class SpriteFontExtensions
    {
        public static RelmSpriteFont ToRelmSpriteFont(this SpriteFont spriteFont)
        {
            return new RelmSpriteFont(spriteFont);
        }
    }
}