using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.UI.Configuration
{
    public class ButtonConfig
        : IConfig
    {
        public Texture2D Texture { get; set; }
        public Vector2 SourceSize { get; set; }
    }
}