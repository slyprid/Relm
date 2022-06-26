using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Extensions;
using Relm.Graphics;
using Relm.Input;
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
            _inputManager = RegisterManager<InputManager>(_viewportAdapter);
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
            ClearScreen();

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

        public T RegisterManager<T>(params object[] args)
            where T : Manager
        {
            var manager = (T)Activator.CreateInstance(typeof(T), args);
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

        public void MapActionToMouseDown(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseButtonDown(button, action);
        }

        public void MapActionToMouseUp(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseButtonUp(button, action);
        }

        public void MapActionToMouseClicked(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseButtonClicked(button, action);
        }

        public void MapActionToMouseDoubleClicked(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseButtonDoubleClicked(button, action);
        }

        public void MapActionToMouseMoved(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseMoved(button, action);
        }

        public void MapActionToMouseWheelMoved(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseWheelMoved(button, action);
        }

        public void MapActionToMouseDragStart(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseDragStart(button, action);
        }

        public void MapActionToMouseDrag(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseDrag(button, action);
        }

        public void MapActionToMouseDragEnd(MouseButton button, Action<EventArgs> action)
        {
            _inputManager.MapActionToMouseDragEnd(button, action);
        }

        #endregion

        #region Utility Methods / Functions

        public void ClearScreen()
        {
            GraphicsDevice.Clear(Color.Black);

            var whitePixel = SpriteBatch.GetWhitePixel();

            SpriteBatch.Begin();

            for (var y = 0; y < ScreenHeight; y++)
            {
                var p = ((float)y / (float)ScreenHeight);
                var color = Color.Lerp(Color.FromNonPremultiplied(10, 2, 23, 255), Color.FromNonPremultiplied(189, 7, 154, 255), p);
                SpriteBatch.Draw(whitePixel, new Rectangle(0, y, ScreenWidth, y), color);
            }

            SpriteBatch.End();
        }

        #endregion
    }
}
