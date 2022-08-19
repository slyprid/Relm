using System.Xml.Serialization;

namespace Relm.Content.Pipeline.BitmapFonts
{
    public class BitmapFontPage
    {
		[XmlAttribute("id")]
        public int id;

        [XmlAttribute("file")]
        public string file;

        /// <summary>
        /// not part of fnt spec. this is manually added.
        /// </summary>
        [XmlAttribute("x")]
        public int x;

        /// <summary>
        /// not part of fnt spec. this is manually added.
        /// </summary>
        [XmlAttribute("y")]
        public int y;
	}
}