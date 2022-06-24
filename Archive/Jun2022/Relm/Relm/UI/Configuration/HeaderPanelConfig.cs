using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class HeaderPanelConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Pieces { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int HeaderHeight { get; set; }

        public HeaderPanelConfig()
        {
            Pieces = new Dictionary<string, Rectangle>();
        }

        public HeaderPanelConfig With(PanelPiece piece, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Pieces.Add(piece.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public HeaderPanelConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }

        public HeaderPanelConfig SetHeaderHeight(int headerHeight)
        {
            HeaderHeight = headerHeight;
            return this;
        }
    }
}