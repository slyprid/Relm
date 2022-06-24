using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class SliderConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public SliderConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public SliderConfig With(SliderPieces piece, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(piece.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public SliderConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}