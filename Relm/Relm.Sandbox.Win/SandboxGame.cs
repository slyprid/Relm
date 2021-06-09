using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Sandbox.Win
{
    public class SandboxGame 
        : RelmGame
    {
        private Texture2D _texture;

        public SandboxGame()
            : base("Relm Sandbox", 1280, 1024, 640, 480)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            _texture = Content.Load<Texture2D>("Test-64");
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

                SpriteBatch.Draw(_texture, Vector2.Zero, Color.White);

            SpriteBatch.End();
        }
    }
}