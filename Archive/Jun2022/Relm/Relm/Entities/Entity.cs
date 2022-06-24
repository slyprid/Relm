using Microsoft.Xna.Framework;

namespace Relm.Entities
{
    public abstract class Entity
        : IEntity
    {
        public abstract void Update(GameTime gameTime);
    }
}