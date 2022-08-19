using System;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public static class InputUtils
    {
        public static bool IsMac;
        public static bool IsWindows;
        public static bool IsLinux;


        static InputUtils()
        {
            IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            IsLinux = Environment.OSVersion.Platform == PlatformID.Unix;
            IsMac = Environment.OSVersion.Platform == PlatformID.MacOSX;
        }


        public static bool IsShiftDown()
        {
            return RelmInput.IsKeyDown(Keys.LeftShift) || RelmInput.IsKeyDown(Keys.RightShift);
        }


        public static bool IsAltDown()
        {
            return RelmInput.IsKeyDown(Keys.LeftAlt) || RelmInput.IsKeyDown(Keys.RightAlt);
        }


        public static bool IsControlDown()
        {
            if (IsMac)
                return RelmInput.IsKeyDown(Keys.LeftWindows) || RelmInput.IsKeyDown(Keys.RightWindows);

            return RelmInput.IsKeyDown(Keys.LeftControl) || RelmInput.IsKeyDown(Keys.RightControl);
        }
    }
}