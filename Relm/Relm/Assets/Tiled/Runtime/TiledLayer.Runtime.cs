using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Collections;

namespace Relm.Assets.Tiled
{
	public partial class TiledLayer 
        : ITiledLayer
	{
		public TiledLayerTile GetTile(int x, int y) => Tiles[x + y * Width];

		public TiledLayerTile GetTileAtWorldPosition(Vector2 pos)
		{
			var worldPoint = Map.WorldToTilePosition(pos);
			return GetTile(worldPoint.X, worldPoint.Y);
		}

		public List<Rectangle> GetCollisionRectangles()
		{
			var checkedIndexes = new bool?[Tiles.Length];
			var rectangles = new List<Rectangle>();
			var startCol = -1;
			var index = -1;

			for (var y = 0; y < Map.Height; y++)
			{
				for (var x = 0; x < Map.Width; x++)
				{
					index = y * Map.Width + x;
					var tile = GetTile(x, y);

					if (tile != null && (checkedIndexes[index] == false || checkedIndexes[index] == null))
					{
						if (startCol < 0)
							startCol = x;

						checkedIndexes[index] = true;
					}
					else if (tile == null || checkedIndexes[index] == true)
					{
						if (startCol >= 0)
						{
							rectangles.Add(FindBoundsRect(startCol, x, y, checkedIndexes));
							startCol = -1;
						}
					}
				} // end for x

				if (startCol >= 0)
				{
					rectangles.Add(FindBoundsRect(startCol, Map.Width, y, checkedIndexes));
					startCol = -1;
				}
			}

			return rectangles;
		}

		public Rectangle FindBoundsRect(int startX, int endX, int startY, bool?[] checkedIndexes)
		{
			var index = -1;

			for (var y = startY + 1; y < Map.Height; y++)
			{
				for (var x = startX; x < endX; x++)
				{
					index = y * Map.Width + x;
					var tile = GetTile(x, y);

					if (tile == null || checkedIndexes[index] == true)
					{
						for (var _x = startX; _x < x; _x++)
						{
							index = y * Map.Width + _x;
							checkedIndexes[index] = false;
						}

						return new Rectangle(startX * Map.TileWidth, startY * Map.TileHeight,
							(endX - startX) * Map.TileWidth, (y - startY) * Map.TileHeight);
					}

					checkedIndexes[index] = true;
				}
			}

			return new Rectangle(startX * Map.TileWidth, startY * Map.TileHeight,
				(endX - startX) * Map.TileWidth, (Map.Height - startY) * Map.TileHeight);
		}

		public List<TiledLayerTile> GetTilesIntersectingBounds(Rectangle bounds)
		{
			var minX = Map.WorldToTilePositionX(bounds.X);
			var minY = Map.WorldToTilePositionY(bounds.Y);
			var maxX = Map.WorldToTilePositionX(bounds.Right);
			var maxY = Map.WorldToTilePositionY(bounds.Bottom);

			var tilelist = ListPool<TiledLayerTile>.Obtain();

			for (var x = minX; x <= maxX; x++)
			{
				for (var y = minY; y <= maxY; y++)
				{
					var tile = GetTile(x, y);
					if (tile != null)
						tilelist.Add(tile);
				}
			}

			return tilelist;
		}

		public TiledLayerTile SetTile(TiledLayerTile tile)
		{
			Tiles[tile.X + tile.Y * Width] = tile;
			tile.Tileset = Map.GetTilesetForTileGid(tile.Gid);

			return tile;
		}

		public void RemoveTile(int x, int y) => Tiles[x + y * Width] = null;
	}
}