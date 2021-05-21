using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.UI
{
    public class Skin
    {
        public string Name { get; set; }
        public Type SkinType { get; set; }
        public Dictionary<string, Rectangle> Parts { get; set; }
        public string FontName { get; set; }

        public SpriteFont Font => Relm.States.GameState.Fonts[FontName];

        public Rectangle this[string name] => Parts[name];

        public Skin()
        {
            Parts = new Dictionary<string, Rectangle>();
        }

        public Rectangle AddPart(string name, int x, int y, int width, int height)
        {
            var ret = new Rectangle(x, y, width, height);
            Parts.Add(name, ret);
            return ret;
        }

        public void SetFont(string name)
        {
            FontName = name;
        }
    }
}