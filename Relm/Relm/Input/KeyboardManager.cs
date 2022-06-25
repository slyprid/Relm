using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    internal class KeyboardManager
    {
        private readonly KeyboardSettings _settings;
        private readonly Array _keysValues = Enum.GetValues(typeof(Keys));
        private bool _isInitial;
        private TimeSpan _lastPressTime;
        private Keys _previousKey;
        private KeyboardState _previousState;

        public event EventHandler<KeyboardEventArgs> KeyTyped;
        public event EventHandler<KeyboardEventArgs> KeyPressed;
        public event EventHandler<KeyboardEventArgs> KeyReleased;

        public bool RepeatPress => _settings.RepeatPress;
        public int InitialDelay => _settings.InitialDelay;
        public int RepeatDelay => _settings.RepeatDelay;

        public KeyboardManager()
        {
            _settings = new KeyboardSettings();
        }

        public KeyboardManager(KeyboardSettings settings)
        {
            _settings = settings;
        }

        public void Update(GameTime gameTime)
        {
            var currentState = Keyboard.GetState();

            RaisePressedEvents(gameTime, currentState);
            RaiseReleasedEvents(currentState);

            if (RepeatPress) RaiseRepeatEvents(gameTime, currentState);

            _previousState = currentState;
        }

        private void RaisePressedEvents(GameTime gameTime, KeyboardState currentState)
        {
            if (!currentState.IsKeyDown(Keys.LeftAlt) && !currentState.IsKeyDown(Keys.RightAlt))
            {
                var pressedKeys = _keysValues
                    .Cast<Keys>()
                    .Where(key => currentState.IsKeyDown(key) && _previousState.IsKeyUp(key));
                
                foreach (var key in pressedKeys)
                {
                    var args = new KeyboardEventArgs(key, currentState);

                    KeyPressed?.Invoke(this, args);

                    if (args.Character.HasValue)
                        KeyTyped?.Invoke(this, args);

                    _previousKey = key;
                    _lastPressTime = gameTime.TotalGameTime;
                    _isInitial = true;
                }
            }
        }

        private void RaiseReleasedEvents(KeyboardState currentState)
        {
            var releasedKeys = _keysValues
                .Cast<Keys>()
                .Where(key => currentState.IsKeyUp(key) && _previousState.IsKeyDown(key));

            foreach (var key in releasedKeys)
                KeyReleased?.Invoke(this, new KeyboardEventArgs(key, currentState));
        }

        private void RaiseRepeatEvents(GameTime gameTime, KeyboardState currentState)
        {
            var elapsedTime = (gameTime.TotalGameTime - _lastPressTime).TotalMilliseconds;

            if (currentState.IsKeyDown(_previousKey) &&
                (_isInitial && elapsedTime > InitialDelay || !_isInitial && elapsedTime > RepeatDelay))
            {
                var args = new KeyboardEventArgs(_previousKey, currentState);

                KeyPressed?.Invoke(this, args);

                if (args.Character.HasValue)
                    KeyTyped?.Invoke(this, args);

                _lastPressTime = gameTime.TotalGameTime;
                _isInitial = false;
            }
        }

        public void UpdateSettings(bool repeatPress, int initialDelay, int repeatDelay)
        {
            _settings.RepeatPress = repeatPress;
            _settings.InitialDelay = initialDelay;
            _settings.RepeatDelay = repeatDelay;
        }
    }
}