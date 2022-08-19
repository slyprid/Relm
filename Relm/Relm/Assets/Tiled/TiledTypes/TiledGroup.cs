using System.Collections.Generic;

namespace Relm.Assets.Tiled
{
    public class TiledGroup 
        : ITiledLayer
    {
        public TiledMap map;
        public string Name { get; set; }
        public float Opacity { get; set; }
        public bool Visible { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float ParallaxFactorX { get; set; }
        public float ParallaxFactorY { get; set; }

        public TiledList<ITiledLayer> Layers;

        public TiledList<TiledLayer> TileLayers;
        public TiledList<TiledObjectGroup> ObjectGroups;
        public TiledList<TiledImageLayer> ImageLayers;
        public TiledList<TiledGroup> Groups;

        public Dictionary<string, string> Properties { get; set; }
    }
}