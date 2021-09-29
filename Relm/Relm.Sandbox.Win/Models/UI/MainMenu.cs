using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens.Transitions;
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
            
        }

        public override void Initialize()
        {
            if (IsInitialized) return;

            Input.OnKeyPressed(Keys.Escape, (sender, args) =>
            {
                UserInterface.Change(nameof(Hud), new FadeTransition(GraphicsDevice, Color.Black));
                Relm.Screens.UnPause();
            }, this);

            Controls.Add<Button>(ControlNames.NewGameButton)
                .SetPosition<Button>(Layout.CenterScreen)
                .Offset<Button>(0, Layout.Height - (128 * 3) + (16 * 2))
                .SetText("New Game");

            Controls.Add<Button>(ControlNames.ContinueGameButton)
                .SetPosition<Button>(Layout.CenterScreen)
                .Offset<Button>(0, Layout.Height - (128 * 2) + (16 * 1))
                .SetText("Continue Game");

            Controls.Add<Button>(ControlNames.ExitButton)
                .SetPosition<Button>(Layout.CenterScreen)
                .Offset<Button>(0, Layout.Height - (128 * 1))
                .SetText("Exit")
                .OnClick((sender, args) =>
                {
                    RelmGame.Exit();
                }, this);

            base.Initialize();

            IsInitialized = true;
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