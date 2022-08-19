using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.Assets.Tiled
{
    public class TiledObjectGroup : ITiledLayer
    {
        public TiledMap Map;
        public string Name { get; set; }
        public float Opacity { get; set; }
        public bool Visible { get; set; }
        public float OffsetX { get; set; }
        public float OffsetY { get; set; }
        public float ParallaxFactorX { get; set; }
        public float ParallaxFactorY { get; set; }

        public Color Color;
        public DrawOrderType DrawOrder;

        public TiledList<TiledObject> Objects;
        public Dictionary<string, string> Properties { get; set; }
    }
}