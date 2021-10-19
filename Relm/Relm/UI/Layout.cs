using Microsoft.Xna.Framework;

namespace Relm.UI
{
    public static class Layout
    {
        public static int Width => (int)RelmGame.Camera.BoundingRectangle.Width;
        public static int Height => (int)RelmGame.Camera.BoundingRectangle.Height;
        public static Vector2 CenterScreen => RelmGame.Camera.Center;
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

        public static Vector2 CenteredFont(string fontSetName, int size, string text)
        {
            var fontSet = ContentLibrary.FontSets[fontSetName];
            var font = fontSet[size];
            var txtSize = font.MeasureString(text);
            return CenterScreen - new Vector2(txtSize.X / 2f, txtSize.Y / 2f);
        }
    }
}