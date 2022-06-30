using Microsoft.Xna.Framework;
using Relm.Content;
using Relm.Graphics;

namespace Relm.UserInterface
{
    public static class Layout
    {
        public static ViewportAdapter ViewportAdapter { get; set; }

        public static int Width => (int)ViewportAdapter.VirtualWidth;
        public static int Height => (int)ViewportAdapter.VirtualHeight;
        public static Vector2 CenterScreen => new Vector2(Width / 2, Height / 2);
        public static Vector2 TopLeft => Vector2.Zero;
        public static Vector2 TopCenter => new Vector2(CenterScreen.X, 0);
        public static Vector2 TopRight => new Vector2(Width, 0);
        public static Vector2 CenterLeft => new Vector2(0, CenterScreen.Y);
        public static Vector2 CenterRight => new Vector2(Width, CenterScreen.Y);
        public static Vector2 BottomLeft => new Vector2(0, Height);
        public static Vector2 BottomCenter => new Vector2(CenterScreen.X, Height);
        public static Vector2 BottomRight => new Vector2(Width, Height);

        public static Vector2 Centered(int width, int height)
        {
            return CenterScreen - new Vector2(width / 2f, height / 2f);
        }
    }
}