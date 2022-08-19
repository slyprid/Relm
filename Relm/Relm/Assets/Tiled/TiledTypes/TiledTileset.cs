using System.Collections.Generic;
using Relm.Math;

namespace Relm.Assets.Tiled
{
    public class TiledTileset 
        : TiledDocument, ITiledElement
    {
        public TiledMap Map;
        public int FirstGid;
        public string Name { get; set; }
        public int TileWidth;
        public int TileHeight;
        public int Spacing;
        public int Margin;
        public int? Columns;
        public int? TileCount;

        public Dictionary<int, TiledTilesetTile> Tiles;
        public TiledTileOffset TileOffset;
        public Dictionary<string, string> Properties;
        public TiledImage Image;
        public TiledList<TiledTerrain> Terrains;

        /// <summary>
        /// cache of the source rectangles for each tile
        /// </summary>
        public Dictionary<int, RectangleF> TileRegions;

        public void Update()
        {
            foreach (var kvPair in Tiles)
                kvPair.Value.UpdateAnimatedTiles();
        }

    }
}