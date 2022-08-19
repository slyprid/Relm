using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Graphics.Textures
{
	public class Sprite
	{
		public Texture2D Texture2D;

		public readonly Rectangle SourceRect;

		public readonly RectangleF Uvs;

		public readonly Vector2 Center;

		public Vector2 Origin;


		public Sprite(Texture2D texture, Rectangle sourceRect, Vector2 origin)
		{
			Texture2D = texture;
			SourceRect = sourceRect;
			Center = new Vector2(sourceRect.Width * 0.5f, sourceRect.Height * 0.5f);
			Origin = origin;

			var inverseTexW = 1.0f / Texture2D.Width;
			var inverseTexH = 1.0f / Texture2D.Height;

			Uvs.X = sourceRect.X * inverseTexW;
			Uvs.Y = sourceRect.Y * inverseTexH;
			Uvs.Width = sourceRect.Width * inverseTexW;
			Uvs.Height = sourceRect.Height * inverseTexH;
		}

		public Sprite(Texture2D texture, Rectangle sourceRect) : this(texture, sourceRect, sourceRect.GetHalfSize()) { }

		public Sprite(Texture2D texture) : this(texture, new Rectangle(0, 0, texture.Width, texture.Height)) { }

		public Sprite(Texture2D texture, int x, int y, int width, int height) : this(texture, new Rectangle(x, y, width, height)) { }

		public Sprite(Texture2D texture, float x, float y, float width, float height) : this(texture, (int)x, (int)y, (int)width, (int)height) { }

		public void GenerateNinePatchRects(Rectangle renderRect, Rectangle[] destArray, int marginLeft, int marginRight, int marginTop, int marginBottom)
		{
			Assert.IsTrue(destArray.Length == 9, "destArray does not have a length of 9");

			var stretchedCenterWidth = renderRect.Width - marginLeft - marginRight;
			var stretchedCenterHeight = renderRect.Height - marginTop - marginBottom;
			var bottomY = renderRect.Y + renderRect.Height - marginBottom;
			var rightX = renderRect.X + renderRect.Width - marginRight;
			var leftX = renderRect.X + marginLeft;
			var topY = renderRect.Y + marginTop;

			destArray[0] = new Rectangle(renderRect.X, renderRect.Y, marginLeft, marginTop); // top-left
			destArray[1] = new Rectangle(leftX, renderRect.Y, stretchedCenterWidth, marginTop); // top-center
			destArray[2] = new Rectangle(rightX, renderRect.Y, marginRight, marginTop); // top-right

			destArray[3] = new Rectangle(renderRect.X, topY, marginLeft, stretchedCenterHeight); // middle-left
			destArray[4] = new Rectangle(leftX, topY, stretchedCenterWidth, stretchedCenterHeight); // middle-center
			destArray[5] = new Rectangle(rightX, topY, marginRight, stretchedCenterHeight); // middle-right

			destArray[6] = new Rectangle(renderRect.X, bottomY, marginLeft, marginBottom); // bottom-left
			destArray[7] = new Rectangle(leftX, bottomY, stretchedCenterWidth, marginBottom); // bottom-center
			destArray[8] = new Rectangle(rightX, bottomY, marginRight, marginBottom); // bottom-right
		}

		public Sprite Clone()
		{
			return new Sprite(Texture2D, SourceRect)
			{
				Origin = Origin
			};
		}

		public static List<Sprite> SpritesFromAtlas(Texture2D texture, int cellWidth, int cellHeight, int cellOffset = 0, int maxCellsToInclude = int.MaxValue)
		{
			var sprites = new List<Sprite>();

			var cols = texture.Width / cellWidth;
			var rows = texture.Height / cellHeight;
			var i = 0;

			for (var y = 0; y < rows; y++)
			{
				for (var x = 0; x < cols; x++)
				{
					if (i++ < cellOffset)
						continue;

					sprites.Add(new Sprite(texture,
						new Rectangle(x * cellWidth, y * cellHeight, cellWidth, cellHeight)));

					if (sprites.Count == maxCellsToInclude)
						return sprites;
				}
			}

			return sprites;
		}

		public static implicit operator Texture2D(Sprite tex) => tex.Texture2D;

		public override string ToString() => $"{SourceRect}";
	}
}