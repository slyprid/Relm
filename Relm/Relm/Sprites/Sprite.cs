using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Textures;
using Relm.Extensions;

namespace Relm.Sprites
{
    public class Sprite
        : Entity
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Size => new Vector2(Width, Height);
        public Texture2D Texture { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public Color Tint { get; set; }
        public string AtlasRegionName { get; set; }

        public Sprite()
        {
            Tint = Color.White;
        }

        public Sprite(Texture2D texture)
            : this()
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
        }

        public Sprite(TextureAtlas textureAtlas)
            : this()
        {
            TextureAtlas = textureAtlas;
            var firstRegion = TextureAtlas.Regions.First();
            AtlasRegionName = firstRegion.Name;
            Width = firstRegion.Width;
            Height = firstRegion.Height;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                spriteBatch.Draw(Texture, Position, Tint);
            }
            else
            {
                var region = TextureAtlas.GetRegion(AtlasRegionName);
                Width = region.Width;
                Height = region.Height;
                spriteBatch.Draw(region, Position, Tint);
            }
        }
    }
}