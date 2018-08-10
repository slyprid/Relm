using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Interfaces
{
    public interface IDraw
    {
        bool IsVisible { get; set; }
        Vector2 Position { get; set; }
        Vector2 Size { get; set; }

        void Draw(GameTime gameTime);
    }
}