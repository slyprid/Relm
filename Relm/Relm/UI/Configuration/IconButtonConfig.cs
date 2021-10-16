using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.UI.Configuration
{
    public class IconButtonConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public IconButtonConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public IconButtonConfig With(States.ButtonState stateName, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(stateName.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public IconButtonConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}