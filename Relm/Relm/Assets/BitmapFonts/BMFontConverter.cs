using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Assets.BitmapFonts
{
	public static class BMFontConverter
	{
		public static SpriteFont LoadSpriteFontFromBitmapFont(string filename)
		{
			var fontData = BitmapFontLoader.LoadFontFromFile(filename);
			if (fontData.Pages.Length > 1)
				throw new Exception(
					$"Found multiple textures in font file {filename}. Only single texture fonts are supported.");

			var texture = Texture2D.FromStream(RelmGame.GraphicsDevice, File.OpenRead(fontData.Pages[0].Filename));
			return LoadSpriteFontFromBitmapFont(fontData, texture);
		}

		public static SpriteFont LoadSpriteFontFromBitmapFont(BitmapFont font, Texture2D texture)
		{
			var glyphBounds = new List<Rectangle>();
			var cropping = new List<Rectangle>();
			var chars = new List<char>();
			var kerning = new List<Vector3>();

			var characters = font.Characters.Values.OrderBy(c => c.Char);
			foreach (var character in characters)
			{
				var bounds = character.Bounds;
				glyphBounds.Add(bounds);
				cropping.Add(new Rectangle(character.Offset.X, character.Offset.Y, bounds.Width, bounds.Height));
				chars.Add(character.Char);
				kerning.Add(new Vector3(0, character.Bounds.Width, character.XAdvance - character.Bounds.Width));
			}

			var constructorInfo = typeof(SpriteFont).GetTypeInfo().DeclaredConstructors.First();
			var result = (SpriteFont)constructorInfo.Invoke(new object[]
			{
				texture, glyphBounds, cropping,
				chars, font.LineHeight, 0, kerning, ' '
			});

			return result;
		}
	}
}