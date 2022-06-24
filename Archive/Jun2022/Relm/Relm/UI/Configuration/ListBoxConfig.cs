using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class ListBoxConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Pieces { get; set; }
        
        public ListBoxConfig()
        {
            Pieces = new Dictionary<string, Rectangle>();
        }

        public ListBoxConfig With(PanelPiece piece, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Pieces.Add(piece.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }
    }
}