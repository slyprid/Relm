using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens.Transitions;
using Relm.Extensions;
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
            public static string HealthProgressBar = "pbHealth";
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

            Controls.Add<Background>()
                .Using(TextureNames.Background);
            
            var panelWidth = 192;
            var panelHeight = 192;
            Controls.Add<Panel>()
                .SetPosition<Panel>(Layout.Centered(panelWidth, panelHeight))
                .SetSize<Panel>(panelWidth, panelHeight)
                .Offset<Panel>(0, -256);
            
            var buttonWidth = 128;
            var buttonHeight = 64;
            var btnNewGame = Controls.Add<Button>(ControlNames.NewGameButton)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 3) - (16 * 2))
                .SetPosition<Button>(0, 0)
                .SetText("New Game");

            var btnContinueGame = Controls.Add<Button>(ControlNames.ContinueGameButton)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 2) - (16 * 1))
                .SetPosition<Button>(0, buttonHeight + (16 * 1))
                .SetText("Continue Game");

            var btnExitGame = Controls.Add<Button>(ControlNames.ExitButton)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 1))
                .SetPosition<Button>(0, (buttonHeight * 2) + (16 * 2))
                .SetText("Exit")
                .OnClick(button =>
                {
                    RelmGame.Exit();
                });

            Controls.Add<Icon>()
                .SetPosition<Icon>(480, 256)
                .Using(TextureNames.Icons, "Chat", 32, 32)
                .AddRegion("Chat", 96, 0, 32, 32);

            Controls.Add<Image>()
                .SetPosition<Image>(256, 256)
                .Using(TextureNames.Test64)
                .WithColor(Color.Green.WithOpacity(0.05f));

            Controls.Add<Container>()
                .SetPosition<Container>(Layout.Centered(128, 256))
                .Add(ControlNames.NewGameButton, btnNewGame)
                .Add(ControlNames.ContinueGameButton, btnContinueGame)
                .Add(ControlNames.ExitButton, btnExitGame)
                .SetSize<Container>(128, 256);

            Controls.Add<ProgressBar>(ControlNames.HealthProgressBar)
                .SetPosition<ProgressBar>(256, 320)
                .SetSize<ProgressBar>(256, 32)
                .WithFillColor(Color.Red)
                .SetValues(0, 100, 12);

            Controls.Add<Label>("lblValue")
                .SetPosition<Label>(256, 384)
                .Using(FontNames.Default)
                .WithColor(Color.Red)
                .OnUpdate((label) =>
                {
                    label.Text = $"{((ProgressBar) Controls[ControlNames.HealthProgressBar]).Value}";
                });

            Controls.Add<CheckBox>()
                .SetPosition<CheckBox>(256, 448)
                .Using(FontNames.Default)
                .SetText("Cheats enabled")
                .OnClick(checkBox =>
                {
                    var color = checkBox.Checked ? Color.Green : Color.Red;
                    ((Label) Controls["lblValue"]).Color = color;
                });


            Input.OnKeyPressed(Keys.A, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var progressBar = (ProgressBar) Controls[ControlNames.HealthProgressBar];
                progressBar.Value -= 1;

            }, this);

            Input.OnKeyPressed(Keys.D, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var progressBar = (ProgressBar)Controls[ControlNames.HealthProgressBar];
                progressBar.Value += 1;

            }, this);

            base.Initialize();

            IsInitialized = true;
        }

        public override void Update(GameTime gameTime)
        {
            Console.WriteLine($"Value: {((ProgressBar)Controls[ControlNames.HealthProgressBar]).Value}");
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