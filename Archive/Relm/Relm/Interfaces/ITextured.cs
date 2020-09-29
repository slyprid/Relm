using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Interfaces
{
    public interface ITextured
    {
        Texture2D Texture { get; set; }
        Color Color { get; set; }
    }
}