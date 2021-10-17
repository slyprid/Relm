using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;

namespace Relm
{
    public static class Console
    {
        internal static ConsoleComponent ConsoleComponent { get; set; }

        public static void Initialize(string fontName)
        {
            ConsoleComponent.Font = ContentLibrary.Fonts[fontName];
        }

        public static void WriteLine(string msg)
        {
            ConsoleComponent.WriteLine(msg);
        }

        public static void WriteLine(string msg, Color color)
        {
            ConsoleComponent.WriteLine(msg, color);
        }

        public static void Clear()
        {
            ConsoleComponent.Clear();
        }
    }
}