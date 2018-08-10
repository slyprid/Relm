using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Relm
{
    public class Stage 
        : Game
    {
        #region Fields / Properties

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Vector2 Resolution { get; private set; }


        #endregion

        #region Initialization

        public Stage()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Resolution = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Content

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region Update / Draw

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        #endregion

        #region Utilities

        public void ChangeResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;

            _graphics.ApplyChanges();

            Resolution = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        }

        #endregion
    }
}
