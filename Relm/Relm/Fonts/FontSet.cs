using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Fonts
{
    public class FontSet
    {
        private Dictionary<int, SpriteFont> _fonts;

        public string FontName { get; set; }
        public string Path { get; set; }

        public SpriteFont this[int size] => _fonts[size];
        public SpriteFont this[FontSize size] => _fonts[(int)size];

        public FontSet(string path, string fontName)
        {
            _fonts = new Dictionary<int, SpriteFont>();
            FontName = fontName;
            Path = path;
        }

        public void Load(ContentManager content)
        {
            var fontSizes = Enum.GetValues(typeof(FontSize));
            foreach (var size in fontSizes)
            {
                var iSize = ((int)size);
                var filename = $"{Path}/{FontName}{iSize}";
                _fonts.Add(iSize, content.Load<SpriteFont>(filename));
            }
        }

        public static FontSet Load(ContentManager content, string path, string fontName)
        {
            var ret = new FontSet(path, fontName);
            ret.Load(content);
            return ret;
        }
    }
}