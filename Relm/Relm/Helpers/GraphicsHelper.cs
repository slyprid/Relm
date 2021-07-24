using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.Helpers
{
    public static class GraphicsHelper
    {
        public static Dictionary<string, Rectangle> CreateTextureAtlasRegions(int atlasWidth, int atlasHeight, int tileWidth, int tileHeight)
        {
            var tilesPerRow = atlasWidth / tileWidth;
            var tilesPerColumn = atlasHeight / tileHeight;

            var regions = new Dictionary<string, Rectangle>();
            for (var y = 0; y < tilesPerColumn; y++)
            {
                for (var x = 0; x < tilesPerRow; x++)
                {
                    regions.Add($"{x}-{y}", new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight));
                }
            }
            return regions;
        }
    }
}