using System.Xml.Serialization;

namespace Relm.Content.Pipeline.BitmapFonts
{
    public class BitmapFontKerning
    {
		[XmlAttribute("first")]
        public int first;

        [XmlAttribute("second")]
        public int second;

        [XmlAttribute("amount")]
        public int amount;
	}
}