﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Relm.Models;

namespace Relm.Sandbox.Win.Models.Screens
{
    public class TestScreen1
        : RelmGameScreen
    {
        private SandboxGame Game => (SandboxGame)base.Game;

        public override string Name => nameof(TestScreen1);

        private Vector2 _position = new Vector2(50, 50);

        public TestScreen1(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(new Color(16, 139, 204));
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(ContentLibrary.Textures.Get("Test-64"), _position, Color.White);
            Game.SpriteBatch.End();
        }
    }
}