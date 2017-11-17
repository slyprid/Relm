using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Core.Input
{
    public class InputManager
    {
        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;
        private readonly Dictionary<Keys, List<EventHandler>> _registeredKeyboardPressedActions;
        private readonly Dictionary<Keys, List<EventHandler>> _registeredKeyboardHeldActions;

        public InputManager()
        {
            _registeredKeyboardPressedActions = new Dictionary<Keys, List<EventHandler>>();
            _registeredKeyboardHeldActions = new Dictionary<Keys, List<EventHandler>>();
        }

        public void Update(GameTime gameTime)
        {
            UpdateKeyboard(gameTime);
        }

        #region Keyboard

        private void UpdateKeyboard(GameTime gameTime)
        {
            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            foreach (var kvp in _registeredKeyboardPressedActions)
            {
                if (!IsKeyPressed(kvp.Key)) continue;
                foreach (var evt in kvp.Value)
                {
                    evt.Invoke(null, null);
                }
            }

            foreach (var kvp in _registeredKeyboardHeldActions)
            {
                if (!IsKeyHeld(kvp.Key)) continue;
                foreach (var evt in kvp.Value)
                {
                    evt.Invoke(null, null);
                }
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyUp(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return _currentKeyboardState.IsKeyDown(key) && _previousKeyboardState.IsKeyDown(key);
        }

        public bool IsAnyKeyPressed()
        {
            var keys = Enum.GetValues(typeof(Keys));
            return keys.Cast<object>().Any(key => IsKeyPressed((Keys)key));
        }

        public void OnKeyPress(Keys key, EventHandler action)
        {
            if (_registeredKeyboardPressedActions.ContainsKey(key))
            {
                _registeredKeyboardPressedActions[key].Add(action);
            }
            else
            {
                _registeredKeyboardPressedActions.Add(key, new List<EventHandler> { action });
            }
        }

        public void OnKeyHeld(Keys key, EventHandler action)
        {
            if (_registeredKeyboardHeldActions.ContainsKey(key))
            {
                _registeredKeyboardHeldActions[key].Add(action);
            }
            else
            {
                _registeredKeyboardHeldActions.Add(key, new List<EventHandler> { action });
            }
        }

        #endregion
    }
}