using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class ProgressBarConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public ProgressBarConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public ProgressBarConfig With(ProgressBarPiece pieceName, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(pieceName.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public ProgressBarConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}