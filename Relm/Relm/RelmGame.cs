using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;
using Penumbra;
using Relm.Collisions;
using Relm.Components;
using Relm.Graphics;
using Relm.Helpers;
using Relm.Math;
using Relm.UI;
using Relm.Time;
using Relm.UI.Controls;

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
        
        public static Point2 Resolution { get; set; }
        public static DEVMODE1 CurrentDisplayMode { get; set; }
        public SpriteBatch SpriteBatch => _spriteBatch;
        public static OrthographicCamera Camera => _camera;
        public static MessageBox MessageBox { get; set; }
        public static CollisionManager Collisions { get; set; }
        public static FpsComponent FPS { get; set; }
        public static PenumbraComponent Penumbra { get; set; }

        public RelmGame(string title = "", int virtualWidth = 1024, int virtualHeight = 768, int actualWidth = 1024, int actualHeight = 768)
        {
            _instance = this;
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.ApplyChanges();

            Resolution = new Point2(_virtualWidth, _virtualHeight);
            CurrentDisplayMode = new DEVMODE1
            {
                dmPelsWidth = _virtualWidth,
                dmPelsHeight = _virtualHeight,
                IsCustom = true
            };

            Content.RootDirectory = "Content";
            IsMouseVisible = true;   
            
            if(!string.IsNullOrEmpty(title))
            {
                Window.Title = title;
            }

            Random.Initialize();

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

            Timers.TimerManager = new TimerManager();
            Components.Add(Timers.TimerManager);

            Scenarios.ScenarioManager = new ScenarioManager();
            Components.Add(Scenarios.ScenarioManager);

            Collisions = new CollisionManager();
            Collisions.Game = this;
            Components.Add(Collisions);

            FPS = new FpsComponent();
            FPS.Game = this;
            Components.Add(FPS);

            Penumbra = new PenumbraComponent(this);
            Penumbra.Enabled = false;
            Penumbra.Visible = false;
            //Components.Add(Penumbra);

            Input.Register(this, Components);
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = _actualWidth;
            _graphics.PreferredBackBufferHeight = _actualHeight;
            _graphics.ApplyChanges();

            base.Initialize();

            _viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, _virtualWidth, _virtualHeight);
            _camera = new OrthographicCamera(_viewportAdapter);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            ContentLibrary.Content = Content;
            ContentLibrary.GraphicsDevice = GraphicsDevice;
            ContentLibrary.Initialize();
        }

        protected override void Update(GameTime gameTime)
        {
            Input.Update();
            base.Update(gameTime);
            MessageBox?.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            MessageBox?.Draw(gameTime, SpriteBatch);
        }

        public new static void Exit()
        {
            ((Game) _instance).Exit();
        }

        public static void ChangeResolution(ref DEVMODE1 mode)
        {
            if (ResolutionHelper.ChangeResolution(ref mode))
            {
                _instance._viewportAdapter = new BoxingViewportAdapter(_instance.Window, _instance.GraphicsDevice, mode.dmPelsWidth, mode.dmPelsHeight);
                _camera = new OrthographicCamera(_instance._viewportAdapter);
                Resolution = new Point2(mode.dmPelsWidth, mode.dmPelsHeight);
                CurrentDisplayMode = mode;
            }
        }

        public static void ChangeResolution(int width, int height, bool isFullscreen = false)
        {
            _instance._graphics.PreferredBackBufferWidth = width;
            _instance._graphics.PreferredBackBufferHeight = height;
            _instance._graphics.IsFullScreen = isFullscreen;
            _instance._graphics.ApplyChanges();

            _instance._viewportAdapter = new BoxingViewportAdapter(_instance.Window, _instance.GraphicsDevice, width, height);
            _camera = new OrthographicCamera(_instance._viewportAdapter);
        }
    }
}
