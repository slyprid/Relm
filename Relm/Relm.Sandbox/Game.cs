using Microsoft.Xna.Framework;
using Relm.Sandbox.Scenes;

namespace Relm.Sandbox
{
    public class Game 
        : Stage
    {
        public Game()
        {
            ChangeResolution(1280, 720);
            WindowTitle = "Relm Sandbox";
            IsMouseVisible = true;
        }
        
        protected override void LoadContent()
        {
            AddScene(new TestScene());

            ChangeScene(TestScene.Alias);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
