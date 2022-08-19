using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics;
using Relm.Scenes;

namespace Relm
{
    public class RelmGame
        : Game
    {
        internal static RelmGame _instance;

        private string _windowTitle;
        private Scene _scene;
        private Scene _nextScene;
        
        public new static GraphicsDevice GraphicsDevice { get; set; }
        public new static ContentManager Content { get; set; }

        public static RelmGame Instance => _instance;

        public static Scene Scene
        {
            get => _instance._scene;
            set
            {
                if (_instance._scene == null)
                {
                    _instance._scene = value;
                    _instance.OnSceneChanged();
                    _instance._scene.Begin();
                }
                else
                {
                    _instance._nextScene = value;
                }
            }
        }

        public RelmGame(int width = 1280, int height = 720, bool isFullScreen = false, string windowTitle = "Relm", string contentDirectory = "Content")
        {
            _windowTitle = windowTitle;

            _instance = this;

            Screen.Initialize(new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                IsFullScreen = isFullScreen,
                SynchronizeWithVerticalRetrace = true,
                PreferHalfPixelOffset = true
            });

            base.Content.RootDirectory = contentDirectory;
            Content = base.Content;
            IsMouseVisible = true;
            IsFixedTimeStep = false;

            Window.Title = _windowTitle;
        }

        protected override void Initialize()
        {
            base.Initialize();

            GraphicsDevice = base.GraphicsDevice;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        private void OnSceneChanged()
        {
            GC.Collect();
        }
    }
}