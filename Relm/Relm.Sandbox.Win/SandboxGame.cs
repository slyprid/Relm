using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
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

        protected override void Initialize()
        {
            base.Initialize();

            Input.OnKeyPressed(Keys.Escape, (sender, args) => { Exit(); });
            Input.OnKeyPressed(Keys.D1, (sender, args) => { Screens.Change(nameof(TestScreen1), new FadeTransition(GraphicsDevice, Color.Black)); });
            Input.OnKeyPressed(Keys.D2, (sender, args) => { Screens.Change(nameof(TestScreen2), new FadeTransition(GraphicsDevice, Color.Black)); });
            Input.OnKeyPressed(Keys.D3, (sender, args) => { Input.ClearKeyboardEvents(); });

            Input.OnMouseDoubleClicked(MouseButton.Right, (sender, args) => { Exit(); });
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
            base.Update(gameTime);
        }
    }
}