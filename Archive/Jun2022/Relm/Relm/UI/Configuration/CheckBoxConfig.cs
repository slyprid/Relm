using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.UI.States;

namespace Relm.UI.Configuration
{
    public class CheckBoxConfig
        : IConfig
    {
        public Dictionary<string, Rectangle> Regions { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        
        public CheckBoxConfig()
        {
            Regions = new Dictionary<string, Rectangle>();
        }

        public CheckBoxConfig With(CheckBoxState stateName, int srcX, int srcY, int srcWidth, int srcHeight)
        {
            Regions.Add(stateName.ToString(), new Rectangle(srcX, srcY, srcWidth, srcHeight));
            return this;
        }

        public CheckBoxConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}