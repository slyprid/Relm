using System;
using Microsoft.Xna.Framework;
using Relm.Entities;

namespace Relm.Maps
{
    public abstract class MapLayer
        : DrawableEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        protected MapLayer()
        {
            Id = Guid.NewGuid();
        }

        protected MapLayer(Vector2 size)
        {
            Id = Guid.NewGuid();
            Size = size;
        }

        public virtual void Clear() { }
    }
}