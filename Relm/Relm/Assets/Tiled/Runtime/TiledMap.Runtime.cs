using System;
using Microsoft.Xna.Framework;
using Relm.Math;

namespace Relm.Assets.Tiled
{
	public partial class TiledMap 
        : TiledDocument
	{
		#region Tileset and Layer getters

		public TiledTileset GetTilesetForTileGid(int gid)
		{
			if (gid == 0)
				return null;

			for (var i = Tilesets.Count - 1; i >= 0; i--)
			{
				if (Tilesets[i].FirstGid <= gid)
					return Tilesets[i];
			}

			throw new Exception(string.Format("tile gid {0} was not found in any tileset", gid));
		}

		public TiledTilesetTile GetTilesetTile(int gid)
		{
			for (var i = Tilesets.Count - 1; i >= 0; i--)
			{
				if (Tilesets[i].FirstGid <= gid)
				{
					if (Tilesets[i].Tiles.TryGetValue(gid - Tilesets[i].FirstGid, out var tilesetTile))
						return tilesetTile;
				}
			}

			return null;
		}

		public ITiledLayer GetLayer(string name) => Layers.Contains(name) ? Layers[name] : null;

		public T GetLayer<T>(int index) where T : ITiledLayer => (T)Layers[index];

		public T GetLayer<T>(string name) where T : ITiledLayer => (T)GetLayer(name);

		public TiledObjectGroup GetObjectGroup(string name) => ObjectGroups.Contains(name) ? ObjectGroups[name] : null;

		#endregion

		#region world/local conversion

		public Point WorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
		{
			if (Orientation == OrientationType.Isometric)
			{
				return IsometricWorldToTilePosition(pos, clampToTilemapBounds);
			}

			if (Orientation == OrientationType.Hexagonal)
			{
				return HexagonalWorldToTilePosition(pos, clampToTilemapBounds);
			}

			return new Point(
				WorldToTilePositionX(pos.X, clampToTilemapBounds),
				WorldToTilePositionY(pos.Y, clampToTilemapBounds)
			);
		}

		public int WorldToTilePositionX(float x, bool clampToTilemapBounds = true)
		{
			var tileX = Mathf.FastFloorToInt(x / TileWidth);
			if (!clampToTilemapBounds)
				return tileX;
			return Mathf.Clamp(tileX, 0, Width - 1);
		}

		public int WorldToTilePositionY(float y, bool clampToTilemapBounds = true)
		{
			var tileY = Mathf.FloorToInt(y / TileHeight);
			if (!clampToTilemapBounds)
				return tileY;
			return Mathf.Clamp(tileY, 0, Height - 1);
		}

		public Vector2 TileToWorldPosition(Point pos)
		{
			if (Orientation == OrientationType.Isometric)
			{
				return IsometricTileToWorldPosition(pos);
			}

			if (Orientation == OrientationType.Hexagonal)
			{
				return HexagonalTileToWorldPosition(pos);
			}

			return new Vector2(TileToWorldPositionX((int)pos.X), TileToWorldPositionY((int)pos.Y));
		}

		public int TileToWorldPositionX(int x)
		{
			if (Orientation == OrientationType.Isometric ||
				Orientation == OrientationType.Hexagonal)
			{
				throw new InvalidOperationException(
					"Cannot convert tile position to world position for isometric or hexagonal maps with just an X coordinate."
				);
			}

			return x * TileWidth;
		}

		public int TileToWorldPositionY(int y)
		{
			if (Orientation == OrientationType.Isometric ||
				Orientation == OrientationType.Hexagonal)
			{
				throw new InvalidOperationException(
					"Cannot convert tile position to world position for isometric or hexagonal maps with just an Y coordinate."
				);
			}

			return y * TileHeight;
		}

		struct OffsetHex
		{
			public int q;
			public int r;

			public OffsetHex(int q, int r)
			{
				this.q = q;
				this.r = r;
			}
		}

		struct FractionalCubeHex
		{
			public readonly double q;
			public readonly double r;
			public readonly double s;

			public FractionalCubeHex(double q, double r, double s)
			{
				this.q = q;
				this.r = r;
				this.s = s;
			}

			public CubeHex FractionalCubeHexRound()
			{
				int qi = (int)(System.Math.Round(q));
				int ri = (int)(System.Math.Round(r));
				int si = (int)(System.Math.Round(s));

				double q_diff = System.Math.Abs(qi - q);
				double r_diff = System.Math.Abs(ri - r);
				double s_diff = System.Math.Abs(si - s);

				if (q_diff > r_diff && q_diff > s_diff)
				{
					qi = -ri - si;
				}
				else if (r_diff > s_diff)
				{
					ri = -qi - si;
				}
				else
				{
					si = -qi - ri;
				}

				return new CubeHex(qi, ri, si);
			}
		}

		struct CubeHex
		{
			public int q;
			public int r;
			public int s;

			public CubeHex(int q, int r, int s)
			{
				this.q = q;
				this.r = r;
				this.s = s;
			}
		}

		private FractionalCubeHex ScreenPointToFractionalCubeHexCoordinates(float x, float y)
		{
			var newx = (x - TileWidth / 2) / TileWidth;

			double t1 = y / (TileHeight / 2);
			double t2 = System.Math.Floor(newx + t1);
			double r = System.Math.Floor((System.Math.Floor(t1 - newx) + t2) / 3);
			double q = System.Math.Floor((System.Math.Floor(2 * newx + 1) + t2) / 3) - r;
			double s = -q - r;

			var fractionalCubeHex = new FractionalCubeHex(q, r, s);

			return fractionalCubeHex;
		}

		private Vector2 OffsetHexCoordinatesToScreenPoint(OffsetHex offsetHex)
		{
			var sizex = TileWidth / Mathf.Sqrt(3);
			var sizey = TileHeight / 2;

			var x = sizex * Mathf.Sqrt(3) * (offsetHex.q + 0.5 * (offsetHex.r & 1));
			var y = sizey * (0.0f * offsetHex.q + 3.0f / 2.0f * offsetHex.r);
			return new Vector2((float)x + (TileWidth / 2), y + (TileHeight / 2));
		}

		private OffsetHex ConvertCubeCoordinatesToOffset(CubeHex cubeHex)
		{
			var row = cubeHex.r;
			var col = cubeHex.q + (cubeHex.r - 1 * (cubeHex.r & 1)) / 2;
			return new OffsetHex(row, col);
		}

		private CubeHex ConvertOffsetCoordinatesToCube(OffsetHex offsetHex)
		{
			var q = offsetHex.q - (offsetHex.r - (offsetHex.r & 1)) / 2;
			var r = offsetHex.r;
			var s = -q - r;
			return new CubeHex(q, r, s);
		}

		private Point HexagonalWorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
		{
			return HexagonalWorldToTilePosition(pos.X, pos.Y, clampToTilemapBounds);
		}

		private Point HexagonalWorldToTilePosition(
			float x,
			float y,
			bool clampToTilemapBounds = true
		)
		{
			// Implementation of odd, pointy-top hexagons using https://www.redblobgames.com/grids/hexagons/
			var fractionalCubeHex = ScreenPointToFractionalCubeHexCoordinates(x, y);
			var cubeHex = fractionalCubeHex.FractionalCubeHexRound();
			var offsetHex = ConvertCubeCoordinatesToOffset(cubeHex);

			if (!clampToTilemapBounds)
			{
				return new Point(offsetHex.r, offsetHex.q);
			}

			return new Point(
				Mathf.Clamp(offsetHex.r, 0, Width - 1),
				Mathf.Clamp(offsetHex.q, 0, Height - 1)
			);
		}

		private Vector2 HexagonalTileToWorldPosition(Point pos)
		{
			return HexagonalTileToWorldPosition(pos.X, pos.Y);
		}

		private Vector2 HexagonalTileToWorldPosition(int x, int y)
		{
			var offsetHex = new OffsetHex(x, y);
			var worldPosition = OffsetHexCoordinatesToScreenPoint(offsetHex);

			return worldPosition;
		}

		private Point IsometricWorldToTilePosition(Vector2 pos, bool clampToTilemapBounds = true)
		{
			return IsometricWorldToTilePosition(pos.X, pos.Y, clampToTilemapBounds);
		}

		private Point IsometricWorldToTilePosition(float x, float y, bool clampToTilemapBounds = true)
		{
			x -= (Height - 1) * TileWidth / 2;
			var tileX = Mathf.FastFloorToInt((y / TileHeight) + (x / TileWidth));
			var tileY = Mathf.FastFloorToInt((-x / TileWidth) + (y / TileHeight));
			if (!clampToTilemapBounds)
				return new Point(tileX, tileY);
			return new Point(Mathf.Clamp(tileX, 0, Width - 1), Mathf.Clamp(tileY, 0, Height - 1));
		}

		private Vector2 IsometricTileToWorldPosition(Point pos)
		{
			return IsometricTileToWorldPosition(pos.X, pos.Y);
		}

		private Vector2 IsometricTileToWorldPosition(int x, int y)
		{
			var worldX = x * TileWidth / 2 - y * TileWidth / 2 + (Height - 1) * TileWidth / 2;
			var worldY = y * TileHeight / 2 + x * TileHeight / 2;
			return new Vector2(worldX, worldY);
		}

		public Vector2 ToWorldPosition(Vector2 pos)
		{
			if (Orientation == OrientationType.Orthogonal
				|| Orientation == OrientationType.Unknown)
			{
				return pos;
			}
			else if (Orientation == OrientationType.Isometric)
			{
				var originX = Height * TileWidth / 2;

				var tileX = pos.X / TileHeight;
				var tileY = pos.Y / TileHeight;

				var worldPos = new Vector2(
					(tileX - tileY) * TileWidth / 2 + originX,
					(tileX + tileY) * TileHeight / 2);

				worldPos.Y -= TileHeight / 2;

				return worldPos;
			}
			else if (Orientation == OrientationType.Hexagonal)
			{
				throw new NotImplementedException("Hexagonal map orientation not yet supported for conversion of position to world coordinates.");
			}

			throw new NotImplementedException("Map orientation not supported for conversion of position to world coordinates.");
		}

		#endregion

	}
}