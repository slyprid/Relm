using System.Collections.Generic;
using Relm.UI.States;
using Microsoft.Xna.Framework;

namespace Relm.UI.Configuration
{
    public class PanelConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Pieces { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public PanelConfig()
        {
            Pieces = new Dictionary<string, Rectangle>();
        }

        public PanelConfig With(PanelPiece piece, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Pieces.Add(piece.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public PanelConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}