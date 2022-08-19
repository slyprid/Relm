using Microsoft.Xna.Framework;

namespace Relm.Assets.BitmapFonts
{
    public class Character
    {
        public Rectangle Bounds;

        public int Channel;

        public char Char;

        public Point Offset;

        public int TexturePage;

        public int XAdvance;

        public override string ToString() => Char.ToString();
    }
}