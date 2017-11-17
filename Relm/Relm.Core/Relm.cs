using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Core.Scenes;
using Relm.Core.States;

namespace Relm.Core
{
    public class Relm
        : Game
    {
        public static Relm Instance { get; private set; }

        #region Fields

        private readonly GraphicsDeviceManager _graphics;

        #endregion

        #region Properties

        public SpriteBatch SpriteBatch { get; private set; }

        #endregion

        #region Managers

        private SceneManager _scenes;

        #endregion

        #region Initialization

        public Relm()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            Instance = this;
        }

        protected override void Initialize()
        {
            _scenes = new SceneManager();

            InitializeCustomManagers();

            base.Initialize();
        }

        protected virtual void InitializeCustomManagers()
        {

        }

        #endregion

        #region Loading

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            LoadStates();
            LoadScenes();
            LoadCustomContent();
        }

        protected virtual void LoadStates() { }

        protected virtual void LoadCustomContent() { }

        protected virtual void LoadScenes() { }

        protected override void UnloadContent()
        {

        }

        #endregion

        #region Updating

        protected override void Update(GameTime gameTime)
        {
            var currentScene = CurrentScene();
            if (currentScene != null)
            {
                if (currentScene.IsEnabled
                    && !currentScene.IsPaused)
                {
                    currentScene.Update(gameTime);
                }
            }

            base.Update(gameTime);
        }

        #endregion

        #region Rendering

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var currentScene = CurrentScene();
            if (currentScene != null)
            {
                if (currentScene.IsEnabled
                    && !currentScene.IsPaused
                    && currentScene.IsVisible)
                {
                    currentScene.Draw(gameTime);
                }
            }

            base.Draw(gameTime);
        }

        #endregion

        #region Scene Management

        public T LoadScene<T>(string alias)
            where T : Scene
        {
            var scene = Activator.CreateInstance<T>();
            scene.SpriteBatch = SpriteBatch;
            scene.Content = Content;
            _scenes.Add(alias, scene);
            return scene;
        }

        public T LoadScene<T>()
            where T : Scene
        {
            var scene = Activator.CreateInstance<T>();
            scene.SpriteBatch = SpriteBatch;
            scene.Content = Content;
            //_scenes.Add(scene);
            return scene;
        }

        public void UnloadScene(string alias)
        {
            _scenes.Remove(alias);
        }

        public Scene ChangeScene(string alias)
        {
            _scenes.ChangeScene(alias);
            return _scenes.CurrentScene;
        }

        public Scene CurrentScene()
        {
            return _scenes.CurrentScene;
        }

        #endregion

        #region Utilities

        public SpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatch(GraphicsDevice);
        }

        public void ChangeResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        #endregion
    }
}
