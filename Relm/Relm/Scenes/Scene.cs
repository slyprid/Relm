using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Input;
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
        public List<IEntity> Entities { get; }

        public InputManager Input { get; set; }

        public ContentManager Content { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        protected Scene()
        {
            Name = Guid.NewGuid().ToString();
            IsEnabled = true;
            IsVisible = true;
            Entities = new List<IEntity>();
            Input = new InputManager();
        }

        public virtual void LoadContent()
        {
            IsLoaded = true;
        }

        public IEntity AddEntity(IEntity entity)
        {
            Entities.Add(entity);
            entity.SpriteBatch = SpriteBatch;
            return entity;
        }

        public void RemoveEntity(IEntity entity)
        {
            Entities.Remove(entity);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}