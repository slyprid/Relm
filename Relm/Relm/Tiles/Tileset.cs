using System;
using MonoGame.Extended.TextureAtlases;

namespace Relm.Tiles
{
    public class Tileset
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }
        public TextureAtlas TextureAtlas { get; set; }

        public Tileset()
        {
            Id = Guid.NewGuid();
        }
    }
}