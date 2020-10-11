using System;
using Microsoft.Xna.Framework;

namespace Relm.Extensions
{
    public static class ColorExtensions
    {
        public static Color WithOpacity(this Color color, float opacity)
        {
            return new Color(color.R, color.G, color.B, opacity);
        }

        public static Color FromHex(this string hex)
        {
#if WEB
            return Color.White;
#else
            if (hex.StartsWith("#")) hex = hex.Substring(1);

            if (hex.Length < 6) throw new Exception("Color not valid");

            if (hex.Length > 6)
            {
                return new Color(
                    int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber));
            }
            else
            {
                return new Color(
                    int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber));
            }
#endif
        }
    }
}
