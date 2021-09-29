using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Relm.UI
{
    public abstract class UserInterfaceScreen
        : GameScreen
    {
        public abstract string Name { get; }

        public UserInterfaceManager UserInterfaceManager { get; internal set; }
        public ControlManager Controls { get; set; }

        protected UserInterfaceScreen(Game game)
            : base(game)
        {
            Controls = new ControlManager();
        }

        public override void Update(GameTime gameTime)
        {
            Controls.UpdateInput();
            Controls.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = ((RelmGame) Game).SpriteBatch;
            spriteBatch.Begin();
            Controls.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }
    }
}