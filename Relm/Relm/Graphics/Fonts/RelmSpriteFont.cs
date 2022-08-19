using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Graphics.Fonts
{
	public class RelmSpriteFont 
        : IFont
	{
		public float LineSpacing => _font.LineSpacing;

		SpriteFont _font;
		readonly Dictionary<char, SpriteFont.Glyph> _glyphs;

		Matrix2D _transformationMatrix = Matrix2D.Identity;
		
		public RelmSpriteFont(SpriteFont font)
		{
			_font = font;
			_glyphs = font.GetGlyphs();
		}
		
		public Vector2 MeasureString(string text)
		{
			var source = new FontCharacterSource(text);
			Vector2 size;
			MeasureString(ref source, out size);
			return size;
		}
		
		public Vector2 MeasureString(StringBuilder text)
		{
			var source = new FontCharacterSource(text);
			Vector2 size;
			MeasureString(ref source, out size);
			return size;
		}
		
		void MeasureString(ref FontCharacterSource text, out Vector2 size)
		{
			if (text.Length == 0)
			{
				size = Vector2.Zero;
				return;
			}

			SpriteFont.Glyph? defaultGlyph = null;
			if (_font.DefaultCharacter.HasValue)
				defaultGlyph = _glyphs[_font.DefaultCharacter.Value];

			var width = 0.0f;
			var finalLineHeight = (float)_font.LineSpacing;

			var currentGlyph = SpriteFont.Glyph.Empty;
			var offset = Vector2.Zero;
			var firstGlyphOfLine = true;

			for (var i = 0; i < text.Length; ++i)
			{
				var c = text[i];

				if (c == '\r')
					continue;

				if (c == '\n')
				{
					finalLineHeight = _font.LineSpacing;

					offset.X = 0;
					offset.Y += _font.LineSpacing;
					firstGlyphOfLine = true;
					continue;
				}

				if (!_glyphs.TryGetValue(c, out currentGlyph))
				{
					if (!defaultGlyph.HasValue)
						throw new ArgumentException("Errors.TextContainsUnresolvableCharacters", "text");

					currentGlyph = defaultGlyph.Value;
				}

				if (firstGlyphOfLine)
				{
					offset.X = System.Math.Max(currentGlyph.LeftSideBearing, 0);
					firstGlyphOfLine = false;
				}
				else
				{
					offset.X += _font.Spacing + currentGlyph.LeftSideBearing;
				}

				offset.X += currentGlyph.Width;

				var proposedWidth = offset.X + System.Math.Max(currentGlyph.RightSideBearing, 0);
				if (proposedWidth > width)
					width = proposedWidth;

				offset.X += currentGlyph.RightSideBearing;

				if (currentGlyph.Cropping.Height > finalLineHeight)
					finalLineHeight = currentGlyph.Cropping.Height;
			}

			size.X = width;
			size.Y = offset.Y + finalLineHeight;
		}


		public bool TryGetFontRegionForChar(char c, out SpriteFont.Glyph fontGlyph,
											bool useDefaultRegionIfNotPresent = false)
		{
			if (!_glyphs.TryGetValue(c, out fontGlyph))
			{
				if (useDefaultRegionIfNotPresent)
				{
					fontGlyph = _glyphs[_font.DefaultCharacter.Value];
					return true;
				}

				return false;
			}

			return true;
		}


		public bool HasCharacter(char c)
		{
			SpriteFont.Glyph fontGlyph;
			return TryGetFontRegionForChar(c, out fontGlyph);
		}


		#region drawing

		void IFont.DrawInto(SpriteBatch spriteBatch, string text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
		{
			var source = new FontCharacterSource(text);
			DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effect, depth);
		}


		void IFont.DrawInto(SpriteBatch spriteBatch, StringBuilder text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
		{
			var source = new FontCharacterSource(text);
			DrawInto(spriteBatch, ref source, position, color, rotation, origin, scale, effect, depth);
		}


		public void DrawInto(SpriteBatch spriteBatch, ref FontCharacterSource text, Vector2 position, Color color, float rotation, Vector2 origin, Vector2 scale, SpriteEffects effect, float depth)
		{
			var flipAdjustment = Vector2.Zero;

			var flippedVert = (effect & SpriteEffects.FlipVertically) == SpriteEffects.FlipVertically;
			var flippedHorz = (effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;

			if (flippedVert || flippedHorz)
			{
				Vector2 size;
				MeasureString(ref text, out size);

				if (flippedHorz)
				{
					origin.X *= -1;
					flipAdjustment.X = -size.X;
				}

				if (flippedVert)
				{
					origin.Y *= -1;
					flipAdjustment.Y = _font.LineSpacing - size.Y;
				}
			}

			// TODO: This looks excessive... i suspect we could do most of this with simple vector math and avoid this much matrix work.
			var requiresTransformation = flippedHorz || flippedVert || rotation != 0f || scale != Vector2.One;
			if (requiresTransformation)
			{
				Matrix2D temp;
				Matrix2D.CreateTranslation(-origin.X, -origin.Y, out _transformationMatrix);
				Matrix2D.CreateScale((flippedHorz ? -scale.X : scale.X), (flippedVert ? -scale.Y : scale.Y), out temp);
				Matrix2D.Multiply(ref _transformationMatrix, ref temp, out _transformationMatrix);
				Matrix2D.CreateTranslation(flipAdjustment.X, flipAdjustment.Y, out temp);
				Matrix2D.Multiply(ref temp, ref _transformationMatrix, out _transformationMatrix);
				Matrix2D.CreateRotation(rotation, out temp);
				Matrix2D.Multiply(ref _transformationMatrix, ref temp, out _transformationMatrix);
				Matrix2D.CreateTranslation(position.X, position.Y, out temp);
				Matrix2D.Multiply(ref _transformationMatrix, ref temp, out _transformationMatrix);
			}

			// Get the default glyph here once.
			SpriteFont.Glyph? defaultGlyph = null;
			if (_font.DefaultCharacter.HasValue)
				defaultGlyph = _glyphs[_font.DefaultCharacter.Value];

			var currentGlyph = SpriteFont.Glyph.Empty;
			var offset = requiresTransformation ? Vector2.Zero : position - origin;
			var firstGlyphOfLine = true;

			for (var i = 0; i < text.Length; ++i)
			{
				var c = text[i];

				if (c == '\r')
					continue;

				if (c == '\n')
				{
					offset.X = requiresTransformation ? 0f : position.X - origin.X;
					offset.Y += _font.LineSpacing;
					firstGlyphOfLine = true;
					continue;
				}

				if (!_glyphs.TryGetValue(c, out currentGlyph))
				{
					if (!defaultGlyph.HasValue)
						throw new ArgumentException("Errors.TextContainsUnresolvableCharacters", "text");

					currentGlyph = defaultGlyph.Value;
				}

				if (firstGlyphOfLine)
				{
					offset.X += System.Math.Max(currentGlyph.LeftSideBearing, 0);
					firstGlyphOfLine = false;
				}
				else
				{
					offset.X += _font.Spacing + currentGlyph.LeftSideBearing;
				}

				var p = offset;

				if (flippedHorz)
					p.X += currentGlyph.BoundsInTexture.Width;
				p.X += currentGlyph.Cropping.X;

				if (flippedVert)
					p.Y += currentGlyph.BoundsInTexture.Height - _font.LineSpacing;
				p.Y += currentGlyph.Cropping.Y;

				// transform our point if we need to
				if (requiresTransformation)
					Vector2Extensions.Transform(ref p, ref _transformationMatrix, out p);

				var destRect = RectangleExtensions.FromFloats(p.X, p.Y,
					currentGlyph.BoundsInTexture.Width * scale.X,
					currentGlyph.BoundsInTexture.Height * scale.Y);

                spriteBatch.Draw(_font.Texture, destRect, currentGlyph.BoundsInTexture, color, rotation, Vector2.Zero, effect, depth);

				offset.X += currentGlyph.Width + currentGlyph.RightSideBearing;
			}
		}

		#endregion
	}
}