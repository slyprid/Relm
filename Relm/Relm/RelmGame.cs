﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using MonoGame.Extended.ViewportAdapters;

namespace Relm
{
    public class RelmGame 
        : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private ViewportAdapter _viewportAdapter;
        private OrthographicCamera _camera;

        private readonly int _virtualWidth;
        private readonly int _virtualHeight;
        private readonly int _actualWidth;
        private readonly int _actualHeight;

        public Point2 Resolution => new Point2(_virtualWidth, _virtualHeight);
        public SpriteBatch SpriteBatch => _spriteBatch;
        public OrthographicCamera Camera => _camera;

        public RelmGame(string title = "", int virtualWidth = 1024, int virtualHeight = 768, int actualWidth = 1024, int actualHeight = 768)
        {
            _virtualWidth = virtualWidth;
            _virtualHeight = virtualHeight;
            _actualWidth = actualWidth;
            _actualHeight = actualHeight;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = _actualWidth,
                PreferredBackBufferHeight = _actualHeight
            };

            ContentLibrary.Content = Content;
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
            ContentLibrary.Initialize();
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
