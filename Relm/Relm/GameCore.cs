using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Constants;
using Relm.States;

namespace Relm
{
    public abstract class GameCore 
        : Game
    {
        #region Fields / Properties

        private GraphicsDeviceManager _graphics;
        
        public static int ResolutionWidth = 720;
        public static int ResolutionHeight = 1280;
        public static int VirtualWidth = 720;
        public static int VirtualHeight = 1280;
        public static SpriteBatch SpriteBatch { get; set; }
        
        public RenderTarget2D Output { get; set; }
        public RenderTarget2D Backbuffer { get; set; }

        #endregion

        #region Initialization

        protected GameCore(int width = 720, int height = 1280)
        {
            VirtualWidth = width;
            VirtualHeight = height;

            _graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait | DisplayOrientation.PortraitDown,
                PreferredBackBufferWidth = VirtualWidth,
                PreferredBackBufferHeight = VirtualHeight
            };
            
            IsMouseVisible = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        #endregion

        #region Content

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Backbuffer = new RenderTarget2D(GraphicsDevice, VirtualWidth, VirtualHeight);
            Output = new RenderTarget2D(GraphicsDevice, ResolutionWidth, ResolutionHeight);

            GameState.GraphicsDevice = GraphicsDevice;
            GameState.Content = Content;
            GameState.Initialize();
            GameState.InitializeRelm();
        }

        #endregion

        #region Update

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        #endregion

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            var pixel = GameState.Textures[TextureNames.Pixel];

            // Render backbuffer
            GraphicsDevice.SetRenderTarget(Backbuffer);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            SpriteBatch.Begin();
            SpriteBatch.Draw(pixel, new Rectangle(100, 100, 250, 250), Color.Red);
            SpriteBatch.End();

            // Scale backbuffer to output resolution
            GraphicsDevice.SetRenderTarget(Output);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin();
            SpriteBatch.Draw(Backbuffer, new Rectangle(0, 0, ResolutionWidth, ResolutionHeight), new Rectangle(0, 0, (int)VirtualWidth, (int)VirtualHeight), Color.White);
            SpriteBatch.End();

            // Render output to device
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);
            SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
            SpriteBatch.Draw(Output, new Rectangle(0, 0, VirtualWidth, VirtualHeight), new Rectangle(0, 0, (int)ResolutionWidth, (int)ResolutionHeight), Color.White);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
