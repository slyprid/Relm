using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Fonts;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Assets.BitmapFonts
{
    public static class SpriteBatchBitmapFontExtensions
    {
        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position, Color color)
        {
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0f);
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position,
                                      Color color,
                                      float rotation, Vector2 origin, float scale, SpriteEffects effects,
                                      float layerDepth)
        {
            var scaleVec = new Vector2(scale, scale);
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scaleVec, effects, layerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, string text, Vector2 position,
                                      Color color,
                                      float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
                                      float layerDepth)
        {
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, StringBuilder text, Vector2 position,
                                      Color color)
        {
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, 0, Vector2.Zero, new Vector2(1), SpriteEffects.None, 0f);
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, StringBuilder text, Vector2 position,
                                      Color color,
                                      float rotation, Vector2 origin, float scale, SpriteEffects effects,
                                      float layerDepth)
        {
            var scaleVec = new Vector2(scale, scale);
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scaleVec, effects, layerDepth);
        }

        public static void DrawString(this SpriteBatch spriteBatch, BitmapFont bitmapFont, StringBuilder text, Vector2 position,
                                      Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
                                      float layerDepth)
        {
            var source = new FontCharacterSource(text);
            bitmapFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effects, layerDepth);
        }

        public static void DrawGlyphs(this SpriteBatch spriteBatch, BitmapFontEnumerator glyphs, Vector2 position, Color color,
                                      float rotation, Vector2 origin, Vector2 scale, float layerDepth)
        {
            foreach (var glyph in glyphs)
            {
                var characterOrigin = origin - glyph.Position;
                RelmGraphics.Instance.SpriteBatch.Draw(glyph.Texture, position, glyph.Character.Bounds, color, rotation, characterOrigin, scale, SpriteEffects.None, layerDepth);
            }
        }
    }
}