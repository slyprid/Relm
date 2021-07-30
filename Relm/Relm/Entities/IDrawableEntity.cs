using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Relm.Entities
{
    public interface IDrawableEntity
        : IEntity
    {
        Vector2 Size { get; set; }
        Vector2 Position { get; set; }

        int Width { get; }
        int Height { get; }
        int X { get; }
        int Y { get; }

        Rectangle Bounds { get; }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}