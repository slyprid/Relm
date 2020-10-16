﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Scenes;

namespace Relm.Entities
{
    public abstract class Entity
    {
        public abstract string Name { get; }
        public Scene Scene { get; set; }
        public virtual Point Size { get; set; }
        public Point Position { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public Color Tint { get; set; }
        public float Opacity { get; set; }
        public float Scale { get; set; }

        public int X => Position.X;
        public int Y => Position.Y;
        public int Width => Size.X;
        public int Height => Size.Y;

        protected Entity()
        {
            IsVisible = true;
            IsEnabled = true;
            Tint = Color.White;
            Opacity = 1f;
            Scale = 1f;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
        }

        public T As<T>()
            where T : Entity
        {
            return (T) this;
        }
    }
}