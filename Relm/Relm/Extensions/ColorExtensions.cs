using Microsoft.Xna.Framework;

namespace Relm.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithOpacity(this Color color, float opacity)
        {
            return WithOpacity(color, (byte)(opacity * 255));
        }

        public static Color WithOpacity(this Color color, byte opacity)
        {
            return new Color(color.R, color.G, color.B, opacity);
        }
    }
}