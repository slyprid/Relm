using Microsoft.Xna.Framework;

namespace Relm.Constants
{
    public static class ScreenConstants
    {
        public static Rectangle ScreenBounds => new Rectangle(0, 0, GameCore.ResolutionWidth, GameCore.ResolutionHeight);
        public static Rectangle VirtualScreenBounds => new Rectangle(0, 0, GameCore.VirtualWidth, GameCore.VirtualHeight);
    }
}