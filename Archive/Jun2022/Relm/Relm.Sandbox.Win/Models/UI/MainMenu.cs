﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens.Transitions;
using Relm.Extensions;
using Relm.Sandbox.Win.Naming;
using Relm.UI;
using Relm.UI.Controls;
using Relm.UI.Extensions;
using Relm.UI.States;

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
            public static string HorizontalScroll = "hScrollBar";
            public static string VerticalScroll = "vScrollBar";
            public static string List = "listList";
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

            Controls.Add<Background>(this)
                .Using(TextureNames.Background);
            
            var panelWidth = 192;
            var panelHeight = 192;
            Controls.Add<Panel>(this)
                .SetPosition<Panel>(Layout.Centered(panelWidth, panelHeight))
                .SetSize<Panel>(panelWidth, panelHeight)
                .Offset<Panel>(0, -256);
            
            var buttonWidth = 128;
            var buttonHeight = 64;
            var btnNewGame = Controls.Add<Button>(ControlNames.NewGameButton, this)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 3) - (16 * 2))
                .SetPosition<Button>(0, 0)
                .SetText("New Game");

            var btnContinueGame = Controls.Add<Button>(ControlNames.ContinueGameButton, this)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 2) - (16 * 1))
                .SetPosition<Button>(0, buttonHeight + (16 * 1))
                .SetText("Continue Game");

            var btnExitGame = Controls.Add<Button>(ControlNames.ExitButton, this)
                //.SetPosition<Button>(Layout.Centered(buttonWidth, buttonHeight))
                //.Offset<Button>(0, (Layout.Height / 2) - (buttonHeight * 1))
                .SetPosition<Button>(0, (buttonHeight * 2) + (16 * 2))
                .SetText("Exit")
                .OnClick(button =>
                {
                    RelmGame.Exit();
                });

            Controls.Add<Icon>(this)
                .SetPosition<Icon>(480, 256)
                .Using(TextureNames.Icons, "Chat", 32, 32)
                .AddRegion("Chat", 96, 0, 32, 32);

            Controls.Add<Image>(this)
                .SetPosition<Image>(256, 256)
                .Using(TextureNames.Test64)
                .WithColor(Color.Green.WithOpacity(0.05f));

            Controls.Add<Container>(this)
                .SetPosition<Container>(Layout.Centered(128, 256))
                .Add(ControlNames.NewGameButton, this, btnNewGame)
                .Add(ControlNames.ContinueGameButton, this, btnContinueGame)
                .Add(ControlNames.ExitButton, this, btnExitGame)
                .SetSize<Container>(128, 256);

            Controls.Add<ProgressBar>(ControlNames.HealthProgressBar, this)
                .SetPosition<ProgressBar>(256, 320)
                .SetSize<ProgressBar>(256, 32)
                .WithFillColor(Color.Red)
                .SetValues(0, 100, 12);

            Controls.Add<Label>("lblValue", this)
                .SetPosition<Label>(256, 384)
                .Using(FontNames.Default)
                .WithColor(Color.Red)
                .OnUpdate((label) =>
                {
                    label.Text = $"{((ProgressBar) Controls[ControlNames.HealthProgressBar]).Value}";
                });

            Controls.Add<CheckBox>(this)
                .SetPosition<CheckBox>(256, 448)
                .Using(FontNames.Default)
                .SetText("Cheats enabled")
                .OnClick(checkBox =>
                {
                    var color = checkBox.Checked ? Color.Green : Color.Red;
                    ((Label) Controls["lblValue"]).Color = color;
                });

            Controls.Add<TextBox>(this)
                .SetPosition<TextBox>(256, 512)
                .SetSize<TextBox>(256, 32)
                .Using(FontNames.Default);

            Controls.Add<AnimatedImage>(this)
                .SetPosition<AnimatedImage>(784, 256)
                .SetScale<AnimatedImage>(3f)
                .Using(TextureNames.Fire)
                .AddCycle("loop", 10, 26, 0.05f)
                .AddFrames(0, 59);

            Controls.Add<ScrollBar>(ControlNames.HorizontalScroll, this)
                .SetPosition<ScrollBar>(784, 384)
                .SetSize<ScrollBar>(256, 32)
                .SetValues(0, 100, 33)
                .SetOrientation(ScrollBarOrientation.Horizontal);

            Controls.Add<ScrollBar>(ControlNames.VerticalScroll, this)
                .SetPosition<ScrollBar>(784, 448)
                .SetSize<ScrollBar>(32, 256)
                .SetOrientation(ScrollBarOrientation.Vertical);

            var listBox = Controls.Add<ListBox>(ControlNames.List, this)
                .SetPosition<ListBox>(848, 8)
                .SetSize<ListBox>(256, 320)
                .UsingFont(FontNames.Default)
                .AddItem("Item 1")
                .AddItem("Item 2")
                .AddItem("Item 3");

            for (var i = 4; i < 101; i++)
            {
                listBox.AddItem($"Item {i}");
            }

            Input.OnKeyPressed(Keys.A, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var scrollBar = (ScrollBar) Controls[ControlNames.HorizontalScroll];
                scrollBar.Value -= 1;

            }, this);

            Input.OnKeyPressed(Keys.D, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var scrollBar = (ScrollBar)Controls[ControlNames.HorizontalScroll];
                scrollBar.Value += 1;

            }, this);

            Input.OnKeyPressed(Keys.W, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var scrollBar = (ScrollBar)Controls[ControlNames.VerticalScroll];
                scrollBar.Value -= 1;

            }, this);

            Input.OnKeyPressed(Keys.S, (sender, args) =>
            {
                var listener = (KeyboardListener)sender;
                if (!listener.RepeatPress) return;
                var scrollBar = (ScrollBar)Controls[ControlNames.VerticalScroll];
                scrollBar.Value += 1;

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