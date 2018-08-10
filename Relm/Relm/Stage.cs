using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;
using Relm.Scenes;

namespace Relm
{
    public class Stage 
        : Game
    {
        #region Fields / Properties

        public static FrameCounter FrameCounter { get; set; }

        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private string _currentSceneName;
        private string _windowTitle;
        
        public Vector2 Resolution { get; private set; }
        public Dictionary<string, Scene> Scenes { get; }
        public Scene CurrentScene => string.IsNullOrEmpty(_currentSceneName) ? null : Scenes[_currentSceneName];

        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                Window.Title = value;
                _windowTitle = value;
            }
        }


        #endregion

        #region Initialization

        public Stage()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Resolution = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            Scenes = new Dictionary<string, Scene>();
        }

        protected override void Initialize()
        {
            FrameCounter = new FrameCounter();

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
            
        }

        #endregion

        #region Update / Draw

        protected override void Update(GameTime gameTime)
        {
            FrameCounter?.Update(gameTime);

            CurrentScene?.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            FrameCounter?.Draw(gameTime);

            GraphicsDevice.Clear(Color.DarkSlateBlue);

            CurrentScene?.Draw(gameTime);

            base.Draw(gameTime);

            Window.Title = $"{WindowTitle} - [{FrameCounter?.FrameRate} fps]";
        }

        #endregion

        #region Scenes

        public Scene AddScene(Scene scene)
        {
            Scenes.Add(scene.Name, scene);
            scene.Content = Content;
            scene.GraphicsDevice = GraphicsDevice;
            scene.SpriteBatch = _spriteBatch;
            return scene;
        }

        public void ChangeScene(string sceneName)
        {
            _currentSceneName = sceneName;

            if (!CurrentScene.IsLoaded)
            {
                CurrentScene.LoadContent();
            }
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
