using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Input;
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
            Input.BindKey(Keys.Escape, InputAction.Pressed, ExitGame);
        }
        
        public override void LoadContent()
        {
            IsLoaded = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;

            Input?.Update(gameTime);

            foreach (var entity in Entities)
            {
                entity.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!IsVisible) return;

            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

            foreach (var entity in Entities)
            {
                entity.Draw(gameTime);
            }

            SpriteBatch.End();
        }

        #region Actions

        private void ExitGame()
        {
            Stage.Instance.Exit();
        }

        #endregion
    }
}