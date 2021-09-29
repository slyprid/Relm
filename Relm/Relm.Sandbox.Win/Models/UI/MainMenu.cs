using Microsoft.Xna.Framework;
using Relm.Sandbox.Win.Naming;
using Relm.UI;
using Relm.UI.Controls;

namespace Relm.Sandbox.Win.Models.UI
{
    public class MainMenu
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

        public override string Name => nameof(MainMenu);

        private Vector2 _position = new Vector2(50, 50);

        public MainMenu(Game game) 
            : base(game)
        {
            Controls.Add<Button>(ControlNames.NewGameButton)
                .SetPosition(100, 100);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(ContentLibrary.Textures.Get(TextureNames.Test64), new Vector2(128, 128), Color.Blue);
            Game.SpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}