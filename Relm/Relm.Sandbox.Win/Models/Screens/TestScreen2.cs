using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Relm.Models;
using Relm.Sandbox.Win.Naming;

namespace Relm.Sandbox.Win.Models.Screens
{
    public class TestScreen2
        : RelmGameScreen
    {
        private new SandboxGame Game => (SandboxGame)base.Game;
        private Vector2 _position = new Vector2(50, 50);

        public override string Name => nameof(TestScreen2);

        public TestScreen2(Game game) : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            _position = Vector2.Lerp(_position, Mouse.GetState().Position.ToVector2(), 1f * gameTime.GetElapsedSeconds());
        }

        public override void Draw(GameTime gameTime)
        {
            //Game.GraphicsDevice.Clear(Color.White);
            Game.SpriteBatch.Begin();
            Game.SpriteBatch.Draw(ContentLibrary.Textures.Get(TextureNames.Test64), _position, Color.Red);
            Game.SpriteBatch.End();
        }
    }
}