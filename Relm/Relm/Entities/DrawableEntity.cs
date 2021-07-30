using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Entities
{
    public abstract class DrawableEntity
        : IDrawableEntity
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public int Width => (int) Size.X;
        public int Height => (int) Size.Y;
        public int X => (int) Position.X;
        public int Y => (int) Position.Y;

        public Rectangle Bounds => new Rectangle(X, Y, Width, Height);

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
    }
}