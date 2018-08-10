using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Interfaces;

namespace Relm.Scenes
{
    public abstract class Scene
        : IEntity
    {
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Size { get; set; }
        public string Name { get; set; }
        public bool IsLoaded { get; set; }
        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        protected Scene()
        {
            Name = Guid.NewGuid().ToString();
            IsEnabled = true;
            IsVisible = true;
        }

        public virtual void LoadContent()
        {
            IsLoaded = true;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}