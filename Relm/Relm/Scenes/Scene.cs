using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;

namespace Relm.Scenes
{
    public abstract class Scene
    {
        public abstract string Name { get; }
        public Point Size { get; set; }
        public Point Position { get; set; }
        public bool IsVisible { get; set; }
        public bool IsEnabled { get; set; }
        public List<Entity> Entities { get; set; }
        public SpriteBatch SpriteBatch { get; set; }

        public int X => Position.X;
        public int Y => Position.Y;
        public int Width => Size.X;
        public int Height => Size.Y;

        public Entity this[string name] => Entities.SingleOrDefault(x => x.Name == name);

        protected Scene()
        {
            Entities = new List<Entity>();
            IsVisible = true;
            IsEnabled = true;
        }

        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            foreach (var entity in Entities)
            {
                entity.Draw(gameTime, SpriteBatch);
            }

            SpriteBatch.End();
        }

        public void AddEntity(Entity entity)
        {
            Entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
        }
    }
}