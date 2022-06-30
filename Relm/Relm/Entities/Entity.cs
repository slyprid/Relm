using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Entities
{
    public abstract class Entity
    {
        public virtual string Name { get; set; } = Guid.NewGuid().ToString();

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        /// <summary>
        /// Called when entity is created from EntityCollection
        /// </summary>
        public virtual void OnCreate() { }

        /// <summary>
        /// Called when entity is removed from EntityCollection
        /// </summary>
        public virtual void OnDestroy() { }
    }
}