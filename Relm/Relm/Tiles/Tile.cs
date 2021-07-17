using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;

namespace Relm.Tiles
{
    public class Tile
        : DrawableEntity
    {
        public Texture2D Texture { get; set; }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null) return;

            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}