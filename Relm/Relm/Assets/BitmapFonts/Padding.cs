namespace Relm.Assets.BitmapFonts
{
    public struct Padding
    {
        public int Bottom;
        public int Left;
        public int Right;
        public int Top;

        public Padding(int left, int top, int right, int bottom)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public override string ToString() => $"{Left}, {Top}, {Right}, {Bottom}";
    }
}