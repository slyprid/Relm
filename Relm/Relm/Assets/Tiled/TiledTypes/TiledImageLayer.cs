using System.Collections.Generic;

namespace Relm.Assets.Tiled
{
    public class TiledImageLayer 
        : ITiledLayer
    {
        public TiledMap Map;
        public string Name { get; set; }
        public bool Visible { get; set; }
        public float Opacity { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float ParallaxFactorX { get; set; }
        public float ParallaxFactorY { get; set; }

        public int? Width;
        public int? Height;

        public TiledImage Image;

        public Dictionary<string, string> Properties { get; set; }
    }
}