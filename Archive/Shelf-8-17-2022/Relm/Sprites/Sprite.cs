using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;

namespace Relm.Sprites
{
    public class Sprite
        : Component
    {
        private Texture2D _texture;

        public override void Update(GameTime gameTime)
        {
            var transform = Entity.GetComponent<Transform>();
            base.Update(gameTime);
        }
    }
}