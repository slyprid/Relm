using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Scenes;

namespace Relm.Sandbox.Scenes
{
    public class TestScene
        : Scene
    {
        public static string Alias = "TestScene";

        public TestScene()
        {
            Name = Alias;
        }

        public override void LoadContent()
        {
            IsLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsVisible) return;

            GraphicsDevice.Clear(Color.Black);
        }
    }
}