using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;

namespace Relm.Extensions
{
    public static class EntityExtensions
    {
        public static void Update(this EntityCollection entities, GameTime gameTime)
        {
            entities.Values.ForEach(x => x.Update(gameTime));
        }

        public static void Draw(this EntityCollection entities, GameTime gameTime, SpriteBatch spriteBatch)
        {
            entities.Values.ForEach(x => x.Draw(gameTime, spriteBatch));
        }
    }
}