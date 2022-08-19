using System.IO;

namespace Relm.Assets.BitmapFonts
{
    public struct Page
    {
        public string Filename;
        public int Id;

        public Page(int id, string filename)
        {
            Filename = filename;
            Id = id;
        }

        public override string ToString() => $"{Id} ({Path.GetFileName(Filename)})";
    }
}