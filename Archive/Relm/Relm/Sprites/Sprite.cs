using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Interfaces;

namespace Relm.Sprites
{
    public class Sprite
        : IEntity, ITextured
    {
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public SpriteBatch SpriteBatch { get; set; }
        
        public Sprite(Texture2D texture, Vector2 size)
        {
            Texture = texture;
            Size = size;
            Color = Color.White;

            IsEnabled = true;
            IsVisible = true;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        public virtual void Draw(GameTime gameTime)
        {
            if (!IsVisible) return;

            SpriteBatch.Draw(Texture, Position, Color);
        }
    }
}