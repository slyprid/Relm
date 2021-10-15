using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using Relm.Components;
using Relm.Media;
using Relm.UI;

namespace Relm
{
    public class RelmGame 
        : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ViewportAdapter _viewportAdapter;
        private static OrthographicCamera _camera;
        private static RelmGame _instance;

        private readonly int _virtualWidth;
        private readonly int _virtualHeight;
        private readonly int _actualWidth;
        private readonly int _actualHeight;

        public Point2 Resolution => new Point2(_virtualWidth, _virtualHeight);
        public SpriteBatch SpriteBatch => _spriteBatch;
        public static OrthographicCamera Camera => _camera;
        public static VideoPlayer VideoPlayer { get; set; }
        public static Video Video { get; set; }

        public RelmGame(string title = "", int virtualWidth = 1024, int virtualHeight = 768, int actualWidth = 1024, int actualHeight = 768)
        {
            _instance = this;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _actualWidth,
                PreferredBackBufferHeight = _actualHeight
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;   
            
            if(!string.IsNullOrEmpty(title))
            {
                Window.Title = title;
            }

            LoadComponents();
        }

        private void LoadComponents()
        {
            Screens.ScreenManager = new ScreenManager();
            Components.Add(Screens.ScreenManager);

            UserInterface.UserInterfaceManager = new UserInterfaceManager();
            Components.Add(UserInterface.UserInterfaceManager);

            Console.ConsoleComponent = new ConsoleComponent();
            Console.ConsoleComponent.Game = this;
            Components.Add(Console.ConsoleComponent);

            Input.Register(this, Components);
        }

        protected override void Initialize()
        {
            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _virtualWidth, _virtualHeight);
            _camera = new OrthographicCamera(_viewportAdapter);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLibrary.Content = Content;
            ContentLibrary.GraphicsDevice = GraphicsDevice;
            ContentLibrary.Initialize();

            VideoPlayer = new VideoPlayer(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        public new static void Exit()
        {
            ((Game) _instance).Exit();
        }

        protected override void UnloadContent()
        {
            VideoPlayer?.Stop();
            Video?.Dispose();
            VideoPlayer?.Dispose();

            base.UnloadContent();
        }
    }
}
