using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.Assets.Tiled
{
    public partial class TiledMap : TiledDocument, IDisposable
    {
        public string Version;
        public string TiledVersion;
        public int Width;
        public int Height;
        public int WorldWidth => Width * TileWidth;
        public int WorldHeight => Height * TileHeight;
        public int TileWidth;
        public int TileHeight;
        public int? HexSideLength;
        public OrientationType Orientation;
        public StaggerAxisType StaggerAxis;
        public StaggerIndexType StaggerIndex;
        public RenderOrderType RenderOrder;
        public Color BackgroundColor;
        public int? NextObjectID;

        public TiledList<ITiledLayer> Layers;

        public TiledList<TiledTileset> Tilesets;
        public TiledList<TiledLayer> TileLayers;
        public TiledList<TiledObjectGroup> ObjectGroups;
        public TiledList<TiledImageLayer> ImageLayers;
        public TiledList<TiledGroup> Groups;

        public Dictionary<string, string> Properties;

        public int MaxTileWidth;

        public int MaxTileHeight;

        public bool RequiresLargeTileCulling => MaxTileWidth > TileWidth || MaxTileHeight > TileHeight;

        public void Update()
        {
            foreach (var tileset in Tilesets)
                tileset.Update();
        }

        #region IDisposable Support

        bool _isDisposed;

        void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    foreach (var tileset in Tilesets)
                        tileset.Image?.Dispose();

                    foreach (var layer in ImageLayers)
                        layer.Image?.Dispose();
                }

                _isDisposed = true;
            }
        }

        void IDisposable.Dispose() => Dispose(true);

        #endregion
    }
}