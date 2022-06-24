using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class ScrollBarConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public ScrollBarConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public ScrollBarConfig With(ScrollBarPiece pieceName, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(pieceName.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public ScrollBarConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}