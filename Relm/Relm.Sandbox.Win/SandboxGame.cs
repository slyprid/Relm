using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens.Transitions;
using Relm.Sandbox.Win.Models.Screens;

namespace Relm.Sandbox.Win
{
    public class SandboxGame 
        : RelmGame
    {
        public SandboxGame()
            : base("Relm Sandbox", 1280, 720, 1280, 1024)
        {
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentLibrary.Textures.Add("Test-64");
            ContentLibrary.Textures.Add("Background");

            Screens.Add(new TestScreen1(this));
            Screens.Add(new TestScreen2(this));
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();
            if (keyboardState.IsKeyDown(Keys.D1))
            {
                Screens.Change(nameof(TestScreen1), new FadeTransition(GraphicsDevice, Color.Black));
            }
            else if (keyboardState.IsKeyDown(Keys.D2))
            {
                Screens.Change(nameof(TestScreen2), new FadeTransition(GraphicsDevice, Color.Black));
            }
            base.Update(gameTime);
        }
    }
}