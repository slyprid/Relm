using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Graphics;

namespace Relm
{
    public class RelmGame 
        : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private ScalingViewportAdapter _viewportAdapter;

        protected SpriteBatch SpriteBatch;

        public int VirtualWidth { get; set; } = 1920;
        public int VirtualHeight { get; set; } = 1080;

        public int ScreenWidth { get; set; } = 1280;
        public int ScreenHeight { get; set; } = 720;
        public ScalingViewportAdapter ViewportAdapter => _viewportAdapter;

        public RelmGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            base.Initialize();
            _viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, VirtualWidth, VirtualHeight);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

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
    }
}
