using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Fonts
{
    public class FontSet
    {
        private readonly Dictionary<int, SpriteFont> _fonts = new Dictionary<int, SpriteFont>();

        public SpriteFont this[int key] => _fonts[key];

        public string Name { get; set; }

        public void Add(int key, string fontName)
        {
            var font = ContentLibrary.Fonts[fontName];
            _fonts.Add(key, font);
        }
    }
}