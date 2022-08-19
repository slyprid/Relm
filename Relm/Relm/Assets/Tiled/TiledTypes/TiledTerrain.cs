using System.Collections.Generic;

namespace Relm.Assets.Tiled
{
    public class TiledTerrain 
        : ITiledElement
    {
        public string Name { get; set; }
        public int Tile;
        public Dictionary<string, string> Properties;
    }
}