using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Extensions;
using Relm.States;

namespace Relm.Sprites
{
    public class Sprite
        : Entity
    {
        public override string Name { get; }
        public string TextureName { get; set; }

        public Sprite()
        {
            Name = Guid.NewGuid().ToString();
        }

        public Sprite(string name)
        {
            Name = name;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(GameState.Textures[TextureName], Position.ToVector2(), Tint.WithOpacity(Opacity));
        }
    }
}