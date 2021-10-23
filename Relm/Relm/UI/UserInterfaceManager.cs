using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Screens.Transitions;

namespace Relm.UI
{
    public class UserInterfaceManager 
        : SimpleDrawableGameComponent
    {
        private UserInterfaceScreen _activeScreen;
        private Transition _activeTransition;

        public UserInterfaceScreen ActiveScreen => _activeScreen;

        public UserInterfaceManager()
        {
            DrawOrder = int.MaxValue;
        }

        public void LoadScreen(UserInterfaceScreen screen, Transition transition)
        {
            if (_activeTransition != null)
                return;
            _activeTransition = transition;
            _activeTransition.StateChanged += (EventHandler)((sender, args) => this.LoadScreen(screen));
            _activeTransition.Completed += (EventHandler)((sender, args) =>
            {
                _activeTransition.Dispose();
                _activeTransition = (Transition)null;
            });
        }

        public void LoadScreen(UserInterfaceScreen screen)
        {
            _activeScreen?.UnloadContent();
            _activeScreen?.Dispose();
            screen.UserInterfaceManager = this;
            screen.Initialize();
            screen.LoadContent();
            _activeScreen = screen;
        }

        public void LoadScreen(UserInterfaceScreen screen, Transition transition, object carryOver)
        {
            if (_activeTransition != null)
                return;
            screen.CarryOver = carryOver;
            _activeTransition = transition;
            _activeTransition.StateChanged += (EventHandler)((sender, args) => this.LoadScreen(screen));
            _activeTransition.Completed += (EventHandler)((sender, args) =>
            {
                _activeTransition.Dispose();
                _activeTransition = (Transition)null;
            });
        }

        public void LoadScreen(UserInterfaceScreen screen, object carryOver)
        {
            screen.CarryOver = carryOver;
            _activeScreen?.UnloadContent();
            _activeScreen?.Dispose();
            screen.UserInterfaceManager = this;
            screen.Initialize();
            screen.LoadContent();
            _activeScreen = screen;
        }

        public override void Initialize()
        {
            base.Initialize();
            _activeScreen?.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _activeScreen?.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            _activeScreen?.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            _activeScreen?.Update(gameTime);
            _activeTransition?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _activeScreen?.Draw(gameTime);
            _activeTransition?.Draw(gameTime);
        }
    }
}