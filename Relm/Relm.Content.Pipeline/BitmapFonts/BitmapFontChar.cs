using System.Xml.Serialization;

namespace Relm.Content.Pipeline.BitmapFonts
{
    public class BitmapFontChar
    {
		[XmlAttribute("id")]
        public int id;

        [XmlAttribute("x")]
        public int x;

        [XmlAttribute("y")]
        public int y;

        [XmlAttribute("width")]
        public int width;

        [XmlAttribute("height")]
        public int height;

        [XmlAttribute("xoffset")]
        public int xOffset;

        [XmlAttribute("yoffset")]
        public int yOffset;

        [XmlAttribute("xadvance")]
        public int xAdvance;

        [XmlAttribute("page")]
        public int page;

        [XmlAttribute("chnl")]
        public int channel;
	}
}