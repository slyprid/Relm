using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.Entities;

namespace Relm.Tiles
{
    public class Tile
        : DrawableEntity
    {
        public Texture2D Texture { get; set; }
        public TextureRegion2D TextureRegion { get; set; }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture == null && TextureRegion == null) return;

            if (Texture != null) spriteBatch.Draw(Texture, Position, Color.White);
            if (TextureRegion != null)
            {
                spriteBatch.Draw(TextureRegion, new Rectangle((int)Position.X, (int)Position.Y, Width, Height), Color.White);
            }
        }

        public static Tile Clone(Tile tile)
        {
            return new Tile
            {
                Position = tile.Position,
                Size = tile.Size,
                Texture = tile.Texture,
                TextureRegion = tile.TextureRegion
            };
        }
    }
}