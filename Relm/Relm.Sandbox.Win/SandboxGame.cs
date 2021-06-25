using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Sandbox.Win
{
    public class SandboxGame 
        : RelmGame
    {
        public SandboxGame()
            : base("Relm Sandbox", 1280, 1024, 640, 480)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentLibrary.Textures.Add("Test-64");
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin(transformMatrix: Camera.GetViewMatrix());

                SpriteBatch.Draw(ContentLibrary.Textures.Get("Test-64"), Vector2.Zero, Color.White);

            SpriteBatch.End();
        }
    }
}