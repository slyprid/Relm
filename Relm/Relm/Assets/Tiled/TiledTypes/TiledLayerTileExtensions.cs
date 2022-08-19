using System.Runtime.CompilerServices;
using Relm.Enumerations;

namespace Relm.Assets.Tiled
{
	public static class TiledLayerTileExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsSlope(this TiledLayerTile self) => self.TilesetTile != null && self.TilesetTile.IsSlope;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsOneWayPlatform(this TiledLayerTile self) => self.TilesetTile != null && self.TilesetTile.IsOneWayPlatform;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetSlopeTopLeft(this TiledLayerTile self)
		{
			if (self.HorizontalFlip && self.VerticalFlip)
				return self.TilesetTile.SlopeTopLeft;
			if (self.HorizontalFlip)
				return self.TilesetTile.SlopeTopRight;
			if (self.VerticalFlip)
				return self.Tileset.Map.TileWidth - self.TilesetTile.SlopeTopLeft;

			return self.TilesetTile.SlopeTopLeft;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int GetSlopeTopRight(this TiledLayerTile self)
		{
			if (self.HorizontalFlip && self.VerticalFlip)
				return self.TilesetTile.SlopeTopRight;
			if (self.HorizontalFlip)
				return self.TilesetTile.SlopeTopLeft;
			if (self.VerticalFlip)
				return self.Tileset.Map.TileWidth - self.TilesetTile.SlopeTopRight;

			return self.TilesetTile.SlopeTopRight;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetSlope(this TiledLayerTile self)
		{
			var tileSize = self.Tileset.Map.TileWidth;

			if (self.HorizontalFlip)
				tileSize *= -1;

			if (self.VerticalFlip)
				tileSize *= -1;

			return (self.TilesetTile.SlopeTopRight - (float)self.TilesetTile.SlopeTopLeft) / tileSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static float GetSlopeOffset(this TiledLayerTile self) => self.GetSlopeTopLeft();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Edge GetHighestSlopeEdge(this TiledLayerTile self)
		{
			var left = self.GetSlopeTopLeft();
			var right = self.GetSlopeTopRight();
			return right > left ? Edge.Left : Edge.Right;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Edge GetNearestEdge(this TiledLayerTile self, int worldPosition)
		{
			var tileWidth = self.Tileset.Map.TileWidth;
			var tileMiddleWorldPosition = self.X * tileWidth + tileWidth / 2;
			return worldPosition < tileMiddleWorldPosition ? Edge.Left : Edge.Right;
		}
	}
}