using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Input;

namespace Relm.Managers
{
    public class InputManager
        : Manager
    {
        private readonly KeyboardManager _keyboardManager;

        private readonly Dictionary<Keys, List<Action>> _keyboardOnKeyPressedActions;
        private readonly Dictionary<Keys, List<Action>> _keyboardOnKeyReleasedActions;
        private readonly Dictionary<Keys, List<Action>> _keyboardOnKeyTypedActions;

        public InputManager()
        {
            _keyboardManager = new KeyboardManager();

            _keyboardManager.KeyPressed += OnKeyPressed;
            _keyboardManager.KeyReleased += OnKeyReleased;
            _keyboardManager.KeyTyped += OnKeyTyped;

            _keyboardOnKeyPressedActions = new Dictionary<Keys, List<Action>>();
            _keyboardOnKeyReleasedActions = new Dictionary<Keys, List<Action>>();
            _keyboardOnKeyTypedActions = new Dictionary<Keys, List<Action>>();
        }
        
        public override void Update(GameTime gameTime)
        {
            _keyboardManager.Update(gameTime);
        }

        #region Keyboard Manager

        public void MapActionToKeyPressed(Keys key, Action action)
        {
            if (!_keyboardOnKeyPressedActions.ContainsKey(key))
            {
                _keyboardOnKeyPressedActions.Add(key, new List<Action> { action });
            }
            else
            {
                _keyboardOnKeyPressedActions[key].Add(action);
            }
        }

        public void MapActionToKeyReleased(Keys key, Action action)
        {
            if (!_keyboardOnKeyReleasedActions.ContainsKey(key))
            {
                _keyboardOnKeyReleasedActions.Add(key, new List<Action> { action });
            }
            else
            {
                _keyboardOnKeyReleasedActions[key].Add(action);
            }
        }

        public void MapActionToKeyTyped(Keys key, Action action)
        {
            if (!_keyboardOnKeyTypedActions.ContainsKey(key))
            {
                _keyboardOnKeyTypedActions.Add(key, new List<Action> { action });
            }
            else
            {
                _keyboardOnKeyTypedActions[key].Add(action);
            }
        }

        public void ClearRegisteredKeyboardActions()
        {
            _keyboardOnKeyPressedActions.Clear();
            _keyboardOnKeyReleasedActions.Clear();
            _keyboardOnKeyTypedActions.Clear();
        }

        public void ChangeKeyboardSettings(bool repeatPress = true, int initialDelay = 500, int repeatDelay = 50)
        {
            _keyboardManager.UpdateSettings(repeatPress, initialDelay, repeatDelay);
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyPressedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyPressedActions[e.Key].ForEach(action => action.Invoke());
            }
        }

        private void OnKeyReleased(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyReleasedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyReleasedActions[e.Key].ForEach(action => action.Invoke());
            }
        }

        private void OnKeyTyped(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyTypedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyTypedActions[e.Key].ForEach(action => action.Invoke());
            }
        }

        #endregion
    }
}