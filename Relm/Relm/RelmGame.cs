using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Graphics;
using Relm.Managers;

namespace Relm
{
    public class RelmGame 
        : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private ScalingViewportAdapter _viewportAdapter;
        private readonly List<Manager> _managers;
        private InputManager _inputManager;

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

            _managers = new List<Manager>();
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;
            _graphics.ApplyChanges();

            base.Initialize();
            _viewportAdapter = new ScalingViewportAdapter(GraphicsDevice, VirtualWidth, VirtualHeight);

            RegisterManagers();
        }

        private void RegisterManagers()
        {
            _inputManager = RegisterManager<InputManager>();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime)
        {
            UpdateManagers(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        #region Manager Handling

        public T RegisterManager<T>()
            where T : Manager, new()
        {
            var manager = Activator.CreateInstance<T>();
            _managers.Add(manager);
            return manager;
        }

        private void UpdateManagers(GameTime gameTime)
        {
            _managers.ForEach(x => x.Update(gameTime));
        }

        #endregion

        #region Input

        public void MapActionToKeyPressed(Keys key, Action<EventArgs> action)
        {
            _inputManager.MapActionToKeyPressed(key, action);
        }

        public void MapActionToKeyDown(Keys key, Action<EventArgs> action)
        {
            _inputManager.MapActionToKeyDown(key, action);
        }

        public void MapActionToKeyReleased(Keys key, Action<EventArgs> action)
        {
            _inputManager.MapActionToKeyReleased(key, action);
        }

        public void MapActionToKeyTyped(Keys key, Action<EventArgs> action)
        {
            _inputManager.MapActionToKeyTyped(key, action);
        }

        public void ClearRegisteredKeyboardActions()
        {
            _inputManager.ClearRegisteredKeyboardActions();
        }

        public void ChangeKeyboardSettings(bool repeatPress = true, int initialDelay = 500, int repeatDelay = 50)
        {
            _inputManager.ChangeKeyboardSettings(repeatPress, initialDelay, repeatDelay);
        }

        public void MapActionToGamepadButtonDown(Buttons button, Action<EventArgs> action)
        {
            _inputManager.MapActionToGamepadButtonDown(button, action);
        }
        
        public void MapActionToGamepadButtonUp(Buttons button, Action<EventArgs> action)
        {
            _inputManager.MapActionToGamepadButtonUp(button, action);
        }

        public void MapActionToGamepadButtonRepeated(Buttons button, Action<EventArgs> action)
        {
            _inputManager.MapActionToGamepadButtonRepeated(button, action);
        }

        public void MapActionToGamepadThumbstickMoved(Buttons button, Action<EventArgs> action)
        {
            _inputManager.MapActionToGamepadThumbstickMoved(button, action);
        }

        public void MapActionToGamepadTriggerMoved(Buttons button, Action<EventArgs> action)
        {
            _inputManager.MapActionToGamepadTriggerMoved(button, action);
        }

        #endregion
    }
}
