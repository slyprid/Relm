using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens.Transitions;
using Relm.Sandbox.Win.Naming;
using Relm.UI;

namespace Relm.Sandbox.Win.Models.UI
{
    public class Hud
        : UserInterfaceScreen
    {
        private new SandboxGame Game => (SandboxGame)base.Game;

        private static class ControlNames
        {
            public static string NewGameButton = "btnNewGame";
            public static string ContinueGameButton = "btnContinueGame";
            public static string CreditsButton = "btnCredits";
            public static string ExitButton = "btnExit";
        }

        public override string Name => nameof(Hud);

        private Vector2 _position = new Vector2(50, 50);

        public Hud(Game game)
            : base(game)
        {
            Input.OnKeyPressed(Keys.Escape, (sender, args) =>
            {
                UserInterface.Change(nameof(MainMenu), new FadeTransition(GraphicsDevice, Color.Black)); 
                Relm.Screens.Pause();
            }, this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(ContentLibrary.Textures.Get(TextureNames.Test64), new Vector2(8, 8), Color.Green);
            Game.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}