using Microsoft.Xna.Framework;

namespace Relm.Sandbox
{
    public class Game 
        : Stage
    {
        public Game()
        {
            ChangeResolution(1280, 720);
            Window.Title = "Relm Sandbox";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
        }

        protected override void UnloadContent()
        {
            
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
