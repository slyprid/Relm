using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Extensions;

namespace Relm.Sprites
{
    public class Sprite
        : IDrawableEntity
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public virtual Texture2D Texture { get; }
        public Color Tint { get; set; }
        public float Opacity { get; set; }

        public virtual int Width => (int)Size.X;
        public virtual int Height => (int)Size.Y;
        public int X => (int)Position.X;
        public int Y => (int)Position.Y;
        public Rectangle Bounds => new Rectangle(X, Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));

        public Sprite()
        {
            IsEnabled = true;
            IsVisible = true;
            Tint = Color.White;
            Opacity = 1f;
            Scale = Vector2.One;
        }

        public Sprite(Texture2D texture) : this()
        {
            Texture = texture;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            spriteBatch.Draw(Texture, Bounds, Tint.WithOpacity(Opacity));
        }
    }
}