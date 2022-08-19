using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Assets.BitmapFonts;
using Relm.Graphics;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Assets.Tiled
{
	public static class TiledRendering
	{
		public static void RenderMap(TiledMap map, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth, RectangleF cameraClipBounds)
		{
			foreach (var layer in map.Layers)
			{
				if (layer is TiledLayer TiledLayer && TiledLayer.Visible)
					RenderLayer(TiledLayer, spriteBatch, position, scale, layerDepth, cameraClipBounds);
				else if (layer is TiledImageLayer TiledImageLayer && TiledImageLayer.Visible)
					RenderImageLayer(TiledImageLayer, spriteBatch, position, scale, layerDepth);
				else if (layer is TiledGroup TiledGroup && TiledGroup.Visible)
					RenderGroup(TiledGroup, spriteBatch, position, scale, layerDepth);
				else if (layer is TiledObjectGroup TiledObjGroup && TiledObjGroup.Visible)
					RenderObjectGroup(TiledObjGroup, spriteBatch, position, scale, layerDepth);
			}
		}

		public static void RenderLayer(ITiledLayer layer, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth, RectangleF cameraClipBounds)
		{
			if (layer is TiledLayer TiledLayer && TiledLayer.Visible)
				RenderLayer(TiledLayer, spriteBatch, position, scale, layerDepth, cameraClipBounds);
			else if (layer is TiledImageLayer TiledImageLayer && TiledImageLayer.Visible)
				RenderImageLayer(TiledImageLayer, spriteBatch, position, scale, layerDepth);
			else if (layer is TiledGroup TiledGroup && TiledGroup.Visible)
				RenderGroup(TiledGroup, spriteBatch, position, scale, layerDepth);
			else if (layer is TiledObjectGroup TiledObjGroup && TiledObjGroup.Visible)
				RenderObjectGroup(TiledObjGroup, spriteBatch, position, scale, layerDepth);
		}

		/// <summary>
		/// renders all tiles with no camera culling performed
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="spriteBatch"></param>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		/// <param name="layerDepth"></param>
		public static void RenderLayer(TiledLayer layer, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth)
		{
			if (!layer.Visible)
				return;

			var tileWidth = layer.Map.TileWidth * scale.X;
			var tileHeight = layer.Map.TileHeight * scale.Y;

			var color = Color.White;
			color.A = (byte)(layer.Opacity * 255);

			for (var i = 0; i < layer.Tiles.Length; i++)
			{
				var tile = layer.Tiles[i];
				if (tile == null)
					continue;

				RenderTile(tile, spriteBatch, position,
					scale, tileWidth, tileHeight,
					color, layerDepth, layer.Map.Orientation,
					layer.Map.Width, layer.Map.Height);
			}
		}

		/// <summary>
		/// renders all tiles that are inside <paramref name="cameraClipBounds"/>
		/// </summary>
		/// <param name="layer"></param>
		/// <param name="spriteBatch"></param>
		/// <param name="position"></param>
		/// <param name="scale"></param>
		/// <param name="layerDepth"></param>
		/// <param name="cameraClipBounds"></param>
		public static void RenderLayer(TiledLayer layer, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth, RectangleF cameraClipBounds)
		{
			if (!layer.Visible)
				return;

			position += layer.Offset;

			// offset it by the entity position since the tilemap will always expect positions in its own coordinate space
			cameraClipBounds.Location -= position;

			var tileWidth = layer.Map.TileWidth * scale.X;
			var tileHeight = layer.Map.TileHeight * scale.Y;

			Point min, max;

			(min, max, cameraClipBounds) = GetLayerCullBounds(layer, scale, cameraClipBounds, tileWidth, tileHeight);

			var color = Color.White;
			color.A = (byte)(layer.Opacity * 255);

			// loop through and draw all the non-culled tiles
			for (var y = min.Y; y <= max.Y; y++)
			{
				for (var x = min.X; x <= max.X; x++)
				{
					var tile = layer.GetTile(x, y);
					if (tile != null)
						RenderTile(tile, spriteBatch, position,
							scale, tileWidth, tileHeight,
							color, layerDepth, layer.Map.Orientation,
							layer.Map.Width, layer.Map.Height);
				}
			}
		}

		private static (Point, Point, RectangleF) GetLayerCullBounds(TiledLayer layer, Vector2 scale, RectangleF cameraClipBounds, float tileWidth, float tileHeight)
		{
			var min = Point.Zero;
			var max = Point.Zero;

			// we expand our cameraClipBounds by the excess tile width/height of the largest tiles to ensure we include tiles whose
			// origin might be outside of the cameraClipBounds
			switch (layer.Map.Orientation)
			{
				case OrientationType.Hexagonal:
				case OrientationType.Isometric:
					if (layer.Map.RequiresLargeTileCulling)
					{
						cameraClipBounds.Location -= new Vector2(
							layer.Map.MaxTileWidth,
							layer.Map.MaxTileHeight - layer.Map.TileWidth
						);
						cameraClipBounds.Size += new Vector2(
							layer.Map.MaxTileWidth,
							layer.Map.MaxTileHeight - layer.Map.TileHeight
						);
					}

					max = new Point(layer.Map.Width - 1, layer.Map.Height - 1);

					break;
				case OrientationType.Staggered:
					throw new NotImplementedException(
						"Staggered Tiled maps are not yet supported."
					);
				case OrientationType.Unknown:
				case OrientationType.Orthogonal:
				default:
					if (layer.Map.RequiresLargeTileCulling)
					{
						min.X = layer.Map.WorldToTilePositionX(cameraClipBounds.Left - (layer.Map.MaxTileWidth * scale.X - tileWidth));
						min.Y = layer.Map.WorldToTilePositionY(cameraClipBounds.Top - (layer.Map.MaxTileHeight * scale.Y - tileHeight));
						max.X = layer.Map.WorldToTilePositionX(cameraClipBounds.Right + (layer.Map.MaxTileWidth * scale.X - tileWidth));
						max.Y = layer.Map.WorldToTilePositionY(cameraClipBounds.Bottom + (layer.Map.MaxTileHeight * scale.Y - tileHeight));
					}
					else
					{
						min.X = layer.Map.WorldToTilePositionX(cameraClipBounds.Left);
						min.Y = layer.Map.WorldToTilePositionY(cameraClipBounds.Top);
						max.X = layer.Map.WorldToTilePositionX(cameraClipBounds.Right);
						max.Y = layer.Map.WorldToTilePositionY(cameraClipBounds.Bottom);
					}
					break;
			}

			return (min, max, cameraClipBounds);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void RenderTile(TiledLayerTile tile, SpriteBatch spriteBatch, Vector2 position,
				Vector2 scale, float tileWidth, float tileHeight,
				Color color, float layerDepth, OrientationType orientation,
				int mapWidth, int mapHeight)
		{
			var gid = tile.Gid;

			// animated tiles (and tiles from image tilesets) will be inside the Tileset itself, in separate TiledTilesetTile
			// objects, not to be confused with TiledLayerTiles which we are dealing with in this loop
			var tilesetTile = tile.TilesetTile;
			if (tilesetTile != null && tilesetTile.AnimationFrames.Count > 0)
				gid = tilesetTile.currentAnimationFrameGid;

			var sourceRect = tile.Tileset.TileRegions[gid];

			// for the y position, we need to take into account if the tile is larger than the tileHeight and shift. Tiled uses
			// a bottom-left coordinate system and MonoGame a top-left
			float tx, ty;

			switch (orientation)
			{
				case OrientationType.Hexagonal:
					bool isEvenRow = tile.Y % 2 == 0;

					if (isEvenRow)
					{
						tx = tile.X * tileWidth;
						ty = tile.Y * tileHeight * 0.75f;
					}
					else
					{
						tx = (tileWidth / 2) + (tile.X * tileWidth);
						ty = tile.Y * tileHeight * 0.75f;
					}

					break;
				case OrientationType.Isometric:
					tx = tile.X * tileWidth / 2 - tile.Y * tileWidth / 2 + (mapHeight - 1) * tileWidth / 2;
					ty = tile.Y * tileHeight / 2 + tile.X * tileHeight / 2;
					break;
				case OrientationType.Staggered:
					throw new NotImplementedException(
						"Staggered Tiled maps are not yet supported."
					);

				case OrientationType.Unknown:
				case OrientationType.Orthogonal:
				default:
					tx = tile.X * tileWidth;
					ty = tile.Y * tileHeight;
					break;
			}

			var rotation = 0f;

			var spriteEffects = SpriteEffects.None;
			if (tile.HorizontalFlip)
				spriteEffects |= SpriteEffects.FlipHorizontally;
			if (tile.VerticalFlip)
				spriteEffects |= SpriteEffects.FlipVertically;
			if (tile.DiagonalFlip)
			{
				if (tile.HorizontalFlip && tile.VerticalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipVertically;
					rotation = MathHelper.PiOver2;
					tx += tileHeight + (sourceRect.Height * scale.Y - tileHeight);
					ty -= (sourceRect.Width * scale.X - tileWidth);
				}
				else if (tile.HorizontalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipVertically;
					rotation = -MathHelper.PiOver2;
					ty += tileHeight;
				}
				else if (tile.VerticalFlip)
				{
					spriteEffects ^= SpriteEffects.FlipHorizontally;
					rotation = MathHelper.PiOver2;
					tx += tileWidth + (sourceRect.Height * scale.Y - tileHeight);
					ty += (tileWidth - sourceRect.Width * scale.X);
				}
				else
				{
					spriteEffects ^= SpriteEffects.FlipHorizontally;
					rotation = -MathHelper.PiOver2;
					ty += tileHeight;
				}
			}

			// if we had no rotations (diagonal flipping) shift our y-coord to account for any non map.tileSize tiles due to
			// Tiled being bottom-left origin
			if (rotation == 0)
				ty += (tileHeight - sourceRect.Height * scale.Y);

			var pos = new Vector2(tx, ty) + position;

			if (tile.Tileset.Image != null)
				spriteBatch.Draw(tile.Tileset.Image.Texture, pos, sourceRect, color, rotation, Vector2.Zero, scale, spriteEffects, layerDepth);
			else
				spriteBatch.Draw(tilesetTile.Image.Texture, pos, sourceRect, color, rotation, Vector2.Zero, scale, spriteEffects, layerDepth);
		}

		public static void RenderObjectGroup(TiledObjectGroup objGroup, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth)
		{
			if (!objGroup.Visible)
				return;

			foreach (var obj in objGroup.Objects)
			{
				if (!obj.Visible)
					continue;

				// if we are not debug rendering, we only render Tile and Text types
				if (!RelmGame.DebugRenderEnabled)
				{
					if (obj.ObjectType != TiledObjectType.Tile && obj.ObjectType != TiledObjectType.Text)
						continue;
				}

				var pos = position + new Vector2(obj.X, obj.Y) * scale;
				switch (obj.ObjectType)
				{
					case TiledObjectType.Basic:
						spriteBatch.DrawHollowRect(pos, obj.Width * scale.X, obj.Height * scale.Y, objGroup.Color);
						goto default;
					case TiledObjectType.Point:
						var size = objGroup.Map.TileWidth * 0.5f;
						pos.X -= size * 0.5f;
						pos.Y -= size * 0.5f;
						spriteBatch.DrawPixel(pos, objGroup.Color, (int)size);
						goto default;
					case TiledObjectType.Tile:
						var tx = obj.Tile.X * objGroup.Map.TileWidth * scale.X;
						var ty = obj.Tile.Y * objGroup.Map.TileHeight * scale.Y;

						var spriteEffects = SpriteEffects.None;
						if (obj.Tile.HorizontalFlip)
							spriteEffects |= SpriteEffects.FlipHorizontally;
						if (obj.Tile.VerticalFlip)
							spriteEffects |= SpriteEffects.FlipVertically;

						var tileset = objGroup.Map.GetTilesetForTileGid(obj.Tile.Gid);
						var sourceRect = tileset.TileRegions[obj.Tile.Gid];
						spriteBatch.Draw(tileset.Image.Texture, pos, sourceRect, Color.White, 0, Vector2.Zero, scale, spriteEffects, layerDepth);
						goto default;
					case TiledObjectType.Ellipse:
						pos = new Vector2(obj.X + obj.Width * 0.5f, obj.Y + obj.Height * 0.5f) * scale;
						spriteBatch.DrawCircle(pos, obj.Width * 0.5f, objGroup.Color);
						goto default;
					case TiledObjectType.Polygon:
					case TiledObjectType.Polyline:
						var points = new Vector2[obj.Points.Length];
						for (var i = 0; i < obj.Points.Length; i++)
							points[i] = obj.Points[i] * scale;
						spriteBatch.DrawPoints(pos, points, objGroup.Color, obj.ObjectType == TiledObjectType.Polygon);
						goto default;
					case TiledObjectType.Text:
						var fontScale = (float)obj.Text.PixelSize / RelmGraphics.Instance.BitmapFont.LineHeight;
						spriteBatch.DrawString(RelmGraphics.Instance.BitmapFont, obj.Text.Value, pos, obj.Text.Color, Mathf.Radians(obj.Rotation), Vector2.Zero, fontScale, SpriteEffects.None, layerDepth);
						goto default;
					default:
						if (RelmGame.DebugRenderEnabled)
							spriteBatch.DrawString(RelmGraphics.Instance.BitmapFont, $"{obj.Name} ({obj.Type})", pos - new Vector2(0, 15), Color.Black);
						break;
				}
			}
		}

		public static void RenderImageLayer(TiledImageLayer layer, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth)
		{
			if (!layer.Visible)
				return;

			var color = Color.White;
			color.A = (byte)(layer.Opacity * 255);

			var pos = position + new Vector2(layer.OffsetX, layer.OffsetY) * scale;
			spriteBatch.Draw(layer.Image.Texture, pos, null, color, 0, Vector2.Zero, scale, SpriteEffects.None, layerDepth);
		}

		public static void RenderGroup(TiledGroup group, SpriteBatch spriteBatch, Vector2 position, Vector2 scale, float layerDepth)
		{
			if (!group.Visible)
				return;

			foreach (var layer in group.Layers)
			{
				if (layer is TiledGroup TiledSubGroup)
					RenderGroup(TiledSubGroup, spriteBatch, position, scale, layerDepth);

				if (layer is TiledObjectGroup TiledObjGroup)
					RenderObjectGroup(TiledObjGroup, spriteBatch, position, scale, layerDepth);

				if (layer is TiledLayer TiledLayer)
					RenderLayer(TiledLayer, spriteBatch, position, scale, layerDepth);

				if (layer is TiledImageLayer TiledImageLayer)
					RenderImageLayer(TiledImageLayer, spriteBatch, position, scale, layerDepth);
			}
		}

	}
}