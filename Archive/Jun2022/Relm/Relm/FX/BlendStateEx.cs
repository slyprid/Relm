using Microsoft.Xna.Framework.Graphics;

namespace Relm.FX
{
    public static class BlendStateEx
    {
        public static BlendState Darken => new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Min
        };

        public static BlendState Multiply => new BlendState
        {
            ColorSourceBlend = Blend.DestinationColor,
            ColorDestinationBlend = Blend.Zero,
            ColorBlendFunction = BlendFunction.Add
        };

        public static BlendState Lighten => new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Max
        };

        public static BlendState LinearDodge => new BlendState
        {
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,
            ColorBlendFunction = BlendFunction.Add
        };
    }
}