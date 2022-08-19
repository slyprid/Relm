using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.Fonts
{
	public static class SpriteBatchSpriteFontExtensions
	{
		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, StringBuilder text,
									  Vector2 position, Color color)
		{
			spriteBatch.DrawString(spriteFont, text, position, color, 0.0f, Vector2.Zero, new Vector2(1.0f),
				SpriteEffects.None, 0.0f);
		}


		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, StringBuilder text,
									  Vector2 position, Color color,
									  float rotation, Vector2 origin, float scale, SpriteEffects effects,
									  float layerDepth)
		{
			spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale), effects,
				layerDepth);
		}


		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, string text, Vector2 position,
									  Color color)
		{
			spriteBatch.DrawString(spriteFont, text, position, color, 0.0f, Vector2.Zero, new Vector2(1.0f),
				SpriteEffects.None, 0.0f);
		}


		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, string text, Vector2 position,
									  Color color, float rotation,
									  Vector2 origin, float scale, SpriteEffects effects, float layerDepth)
		{
			spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, new Vector2(scale), effects,
				layerDepth);
		}


		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, StringBuilder text,
									  Vector2 position, Color color,
									  float rotation, Vector2 origin, Vector2 scale, SpriteEffects effects,
									  float layerDepth)
		{
            Assert.IsFalse(text == null);

			if (text.Length == 0)
				return;

			var source = new FontCharacterSource(text);
			spriteFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effects, layerDepth);
		}


		public static void DrawString(this SpriteBatch spriteBatch, RelmSpriteFont spriteFont, string text, Vector2 position,
									  Color color, float rotation,
									  Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
		{
			Assert.IsFalse(text == null);

			if (text.Length == 0)
				return;

			var source = new FontCharacterSource(text);
			spriteFont.DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effects, layerDepth);
		}
	}
}