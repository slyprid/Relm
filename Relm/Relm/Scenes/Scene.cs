using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Events;
using Relm.UI.Controls;

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
        public EventManager Events { get; private set; }

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
            Events = new EventManager();
        }

        public virtual void OnActivate() { }
        public virtual void OnDeactivate() { }

        public virtual void Cleanup()
        {
            Entities.Clear();
            Events.Clear();
        }

        public virtual void Update(GameTime gameTime)
        {
            Events.Update(gameTime);

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

        public Entity AddEntity(Entity entity)
        {
            entity.Scene = this;
            Entities.Add(entity);
            return entity;
        }

        public Entity AddControl(Control control)
        {
            control.Scene = this;
            Entities.Add(control);
            control.InitializeEvents();
            return control;
        }

        public void RemoveEntity(Entity entity)
        {
            Entities.Remove(entity);
        }

        public Event AddEvent(Event evt)
        {
            evt.Parent = this;
            Events.Add(evt);
            return evt;
        }

        public void RemoveEvent(Event evt)
        {
            Events.Remove(evt);
        }

        public T As<T>()
            where T : Scene
        {
            return (T) this;
        }

        public bool EntityExists(string entityName)
        {
            return Entities.Any(x => x.Name == entityName);
        }
    }
}