using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class TextBoxConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public TextBoxConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public TextBoxConfig With(TextBoxPiece pieceName, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(pieceName.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public TextBoxConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}