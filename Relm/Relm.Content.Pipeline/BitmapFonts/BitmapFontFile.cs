﻿using System.Collections.Generic;
using System.Xml.Serialization;

namespace Relm.Content.Pipeline.BitmapFonts
{
	[XmlRoot("font")]
    public class BitmapFontFile
    {
        /// <summary>
        /// the full path to the fnt font
        /// </summary>
        public string file;

        [XmlElement("info")]
        public BitmapFontInfo info;

        [XmlElement("common")]
        public BitmapFontCommon common;

        [XmlArray("pages")]
        [XmlArrayItem("page")]
        public List<BitmapFontPage> pages;

        [XmlArray("chars")]
        [XmlArrayItem("char")]
        public List<BitmapFontChar> chars;

        [XmlArray("kernings")]
        [XmlArrayItem("kerning")]
        public List<BitmapFontKerning> kernings;
    }
}