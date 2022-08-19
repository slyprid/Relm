using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Fonts;

namespace Relm.Assets.BitmapFonts
{
	public partial class BitmapFont 
        : IDisposable, IFont
	{
		const int kNoMaxWidth = -1;

		#region Properties

		public int AlphaChannel;
        public int BaseHeight;

		public int BlueChannel;

		public bool Bold;

		public IDictionary<char, Character> Characters;

		public string Charset;

		public string FamilyName;

		public int FontSize;

		public int GreenChannel;

		public bool Italic;

		public IDictionary<Kerning, int> Kernings;

		public int LineHeight;

		public float LineSpacing => LineHeight;

		public int OutlineSize;

		public bool Packed;

		public Padding Padding;

		public Page[] Pages;

		public Texture2D[] Textures;

		public int RedChannel;

		public bool Smoothed;

		public Point Spacing;

		public int StretchedHeight;

		public int SuperSampling;

		public Point TextureSize;

		public bool Unicode;

		public Character DefaultCharacter;

		internal int _spaceWidth;

		#endregion

		public Character this[char character] => Characters[character];

		public void Initialize(bool premultiplyAlpha)
		{
			LoadTextures(premultiplyAlpha);
			if (Characters.TryGetValue(' ', out var defaultChar))
			{
				DefaultCharacter = defaultChar;
			}
			else
			{
				Debug.Log($"Font {FamilyName} has no space character!");
				DefaultCharacter = this['a'];
			}

			_spaceWidth = DefaultCharacter.Bounds.Width + DefaultCharacter.XAdvance;
		}

		public int GetKerning(char previous, char current)
		{
			var key = new Kerning(previous, current, 0);
			if (!Kernings.TryGetValue(key, out var result))
				return 0;

			return result;
		}

		public bool ContainsCharacter(char character) => Characters.ContainsKey(character);

		public bool HasCharacter(char character) => ContainsCharacter(character);

		public string WrapText(string text, float maxLineWidth)
		{
			var words = text.Split(' ');
			var sb = new StringBuilder();
			var lineWidth = 0f;

			if (maxLineWidth < _spaceWidth)
				return string.Empty;

			foreach (var word in words)
			{
				var size = MeasureString(word);
				if (lineWidth + size.X < maxLineWidth)
				{
					sb.Append(word + " ");
					lineWidth += size.X + _spaceWidth;
				}
				else
				{
					if (size.X > maxLineWidth)
					{
						if (sb.ToString() == "")
							sb.Append(WrapText(word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
						else
							sb.Append("\n" + WrapText(word.Insert(word.Length / 2, " ") + " ", maxLineWidth));
					}
					else
					{
						sb.Append("\n" + word + " ");
						lineWidth = size.X + _spaceWidth;
					}
				}
			}

			return sb.ToString();
		}

		public string TruncateText(string text, string ellipsis, float maxLineWidth)
		{
			if (maxLineWidth < _spaceWidth)
				return string.Empty;

			var size = MeasureString(text);

			var ellipsisWidth = MeasureString(ellipsis).X;
			if (size.X > maxLineWidth)
			{
				var sb = new StringBuilder();

				var width = 0.0f;
				Character currentChar = null;
				var offsetX = 0.0f;

				for (var i = 0; i < text.Length; i++)
				{
					var c = text[i];

					if (c == '\r' || c == '\n')
						continue;

					if (currentChar != null)
						offsetX += Spacing.X + currentChar.XAdvance;

					if (ContainsCharacter(c))
						currentChar = this[c];
					else
						currentChar = DefaultCharacter;

					var proposedWidth = offsetX + currentChar.XAdvance + Spacing.X;
					if (proposedWidth > width)
						width = proposedWidth;

					if (width < maxLineWidth - ellipsisWidth)
					{
						sb.Append(c);
					}
					else
					{
						sb.Append(ellipsis);
						break;
					}
				}

				return sb.ToString();
			}

			return text;
		}

		public Vector2 MeasureString(string text)
		{
			var result = MeasureString(text, kNoMaxWidth);
			return new Vector2(result.X, result.Y);
		}

		public Vector2 MeasureString(StringBuilder text)
		{
			var result = MeasureString(text, kNoMaxWidth);
			return new Vector2(result.X, result.Y);
		}

		public Point MeasureString(string text, float maxWidth = kNoMaxWidth)
		{
			var source = new FontCharacterSource(text);
			return MeasureString(ref source, maxWidth);
		}

		public Point MeasureString(StringBuilder text, float maxWidth = kNoMaxWidth)
		{
			var source = new FontCharacterSource(text);
			return MeasureString(ref source, maxWidth);
		}

		public Point MeasureString(ref FontCharacterSource text, float maxWidth = kNoMaxWidth)
		{
			if (text.Length == 0)
				return Point.Zero;

			var length = text.Length;
			var previousCharacter = ' ';
			var currentLineWidth = 0;
			var currentLineHeight = LineHeight;
			var blockWidth = 0;
			var blockHeight = 0;
			var lineHeights = new List<int>();

			for (var i = 0; i < length; i++)
			{
				var character = text[i];
				if (character == '\n' || character == '\r')
				{
					if (character == '\n' || i + 1 == length || text[i + 1] != '\n')
					{
						lineHeights.Add(currentLineHeight);
						blockWidth = System.Math.Max(blockWidth, currentLineWidth);
						currentLineWidth = 0;
						currentLineHeight = LineHeight;
					}
				}
				else
				{
					var data = this[character];
					var width = data.XAdvance + GetKerning(previousCharacter, character) + Spacing.X;
					if (maxWidth != kNoMaxWidth && currentLineWidth + width >= maxWidth)
					{
						lineHeights.Add(currentLineHeight);
						blockWidth = System.Math.Max(blockWidth, currentLineWidth);
						currentLineWidth = 0;
						currentLineHeight = LineHeight;
					}

					currentLineWidth += width;
					currentLineHeight = System.Math.Max(currentLineHeight, data.Bounds.Height + data.Offset.Y);
					previousCharacter = character;
				}
			}

			if (currentLineHeight != 0)
				lineHeights.Add(currentLineHeight);

			for (var i = 0; i < lineHeights.Count; i++)
			{
				if (i < lineHeights.Count - 1)
					lineHeights[i] = LineHeight;

				blockHeight += lineHeights[i];
			}

			return new Point(System.Math.Max(currentLineWidth, blockWidth), blockHeight);
		}

		~BitmapFont() => Dispose();

		public void Dispose()
		{
			if (Textures == null)
				return;

			foreach (var tex in Textures)
				tex.Dispose();
			Textures = null;
		}

		public BitmapFontEnumerator GetGlyphs(string text)
		{
			var source = new FontCharacterSource(text);
			return GetGlyphs(ref source);
		}

		public BitmapFontEnumerator GetGlyphs(StringBuilder text)
		{
			var source = new FontCharacterSource(text);
			return GetGlyphs(ref source);
		}

		public BitmapFontEnumerator GetGlyphs(ref FontCharacterSource text) => new BitmapFontEnumerator(this, ref text);
	}
}