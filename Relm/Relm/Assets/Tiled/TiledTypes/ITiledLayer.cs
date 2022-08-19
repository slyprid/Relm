using System.Collections.Generic;

namespace Relm.Assets.Tiled
{
    public interface ITiledLayer 
        : ITiledElement
    {
        float OffsetX { get; }
        float OffsetY { get; }
        float Opacity { get; }
        bool Visible { get; }
        float ParallaxFactorX { get; }
        float ParallaxFactorY { get; }
        Dictionary<string, string> Properties { get; }
    }
}