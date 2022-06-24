using Microsoft.Xna.Framework;

namespace Relm.Extensions
{
    public static class RectangleExtensions
    {
        public static Rectangle ExtendWidthBoth(this Rectangle rect, int value)
        {
            return new Rectangle(rect.X - value, rect.Y, rect.Width + value, rect.Height);
        }

        public static Rectangle ExtendHeightBoth(this Rectangle rect, int value)
        {
            return new Rectangle(rect.X, rect.Y - value, rect.Width, rect.Height + value);
        }
    }
}