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
        private readonly GamePadManager _gamepadManager;

        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyPressedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyReleasedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyTypedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyDownActions;

        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonDownActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonUpActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonRepeatedActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnThumbstickMovedActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnTriggerMovedActions;

        public InputManager()
        {
            _keyboardManager = new KeyboardManager();

            _keyboardManager.KeyPressed += OnKeyPressed;
            _keyboardManager.KeyReleased += OnKeyReleased;
            _keyboardManager.KeyTyped += OnKeyTyped;
            _keyboardManager.KeyDown += OnKeyDown;

            _keyboardOnKeyPressedActions = new Dictionary<Keys, List<Action<EventArgs>>>();
            _keyboardOnKeyReleasedActions = new Dictionary<Keys, List<Action<EventArgs>>>();
            _keyboardOnKeyTypedActions = new Dictionary<Keys, List<Action<EventArgs>>>();
            _keyboardOnKeyDownActions = new Dictionary<Keys, List<Action<EventArgs>>>();
            
            _gamepadManager = new GamePadManager();

            _gamepadManager.ButtonDown += OnGamepadButtonDown;
            _gamepadManager.ButtonUp += OnGamepadButtonUp;
            _gamepadManager.ButtonRepeated += OnGamepadButtonRepeated;
            _gamepadManager.ThumbStickMoved += OnGamepadThumbstickMoved;
            _gamepadManager.TriggerMoved += OnGamepadTriggerMoved;

            _gamepadOnButtonDownActions = new Dictionary<Buttons, List<Action<EventArgs>>>();
            _gamepadOnButtonUpActions = new Dictionary<Buttons, List<Action<EventArgs>>>();
            _gamepadOnButtonRepeatedActions = new Dictionary<Buttons, List<Action<EventArgs>>>();
            _gamepadOnThumbstickMovedActions = new Dictionary<Buttons, List<Action<EventArgs>>>();
            _gamepadOnTriggerMovedActions = new Dictionary<Buttons, List<Action<EventArgs>>>();
        }
        
        public override void Update(GameTime gameTime)
        {
            _keyboardManager.Update(gameTime);
            _gamepadManager.Update(gameTime);
        }

        #region Keyboard Manager

        /// <summary>
        /// Checks if key is pressed based on initial delay and repeating delays
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void MapActionToKeyPressed(Keys key, Action<EventArgs> action)
        {
            if (!_keyboardOnKeyPressedActions.ContainsKey(key))
            {
                _keyboardOnKeyPressedActions.Add(key, new List<Action<EventArgs>> { action });
            }
            else
            {
                _keyboardOnKeyPressedActions[key].Add(action);
            }
        }

        /// <summary>
        /// Checks if key is down
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void MapActionToKeyDown(Keys key, Action<EventArgs> action)
        {
            if (!_keyboardOnKeyDownActions.ContainsKey(key))
            {
                _keyboardOnKeyDownActions.Add(key, new List<Action<EventArgs>> { action });
            }
            else
            {
                _keyboardOnKeyDownActions[key].Add(action);
            }
        }

        public void MapActionToKeyReleased(Keys key, Action<EventArgs> action)
        {
            if (!_keyboardOnKeyReleasedActions.ContainsKey(key))
            {
                _keyboardOnKeyReleasedActions.Add(key, new List<Action<EventArgs>> { action });
            }
            else
            {
                _keyboardOnKeyReleasedActions[key].Add(action);
            }
        }

        public void MapActionToKeyTyped(Keys key, Action<EventArgs> action)
        {
            if (!_keyboardOnKeyTypedActions.ContainsKey(key))
            {
                _keyboardOnKeyTypedActions.Add(key, new List<Action<EventArgs>> { action });
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
            _keyboardOnKeyDownActions.Clear();
        }

        public void ChangeKeyboardSettings(bool repeatPress = true, int initialDelay = 500, int repeatDelay = 50)
        {
            _keyboardManager.UpdateSettings(repeatPress, initialDelay, repeatDelay);
        }

        private void OnKeyPressed(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyPressedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyPressedActions[e.Key].ForEach(action => action.Invoke(e));
            }
        }

        private void OnKeyDown(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyDownActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyDownActions[e.Key].ForEach(action => action.Invoke(e));
            }
        }

        private void OnKeyReleased(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyReleasedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyReleasedActions[e.Key].ForEach(action => action.Invoke(e));
            }
        }

        private void OnKeyTyped(object sender, KeyboardEventArgs e)
        {
            if (_keyboardOnKeyTypedActions.ContainsKey(e.Key))
            {
                _keyboardOnKeyTypedActions[e.Key].ForEach(action => action.Invoke(e));
            }
        }

        #endregion

        #region Gamepad Manager

        public void MapActionToGamepadButtonDown(Buttons button, Action<EventArgs> action)
        {
            if (!_gamepadOnButtonDownActions.ContainsKey(button))
            {
                _gamepadOnButtonDownActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _gamepadOnButtonDownActions[button].Add(action);
            }
        }

        public void MapActionToGamepadButtonUp(Buttons button, Action<EventArgs> action)
        {
            if (!_gamepadOnButtonUpActions.ContainsKey(button))
            {
                _gamepadOnButtonUpActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _gamepadOnButtonUpActions[button].Add(action);
            }
        }

        public void MapActionToGamepadButtonRepeated(Buttons button, Action<EventArgs> action)
        {
            if (!_gamepadOnButtonRepeatedActions.ContainsKey(button))
            {
                _gamepadOnButtonRepeatedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _gamepadOnButtonRepeatedActions[button].Add(action);
            }
        }

        public void MapActionToGamepadThumbstickMoved(Buttons button, Action<EventArgs> action)
        {
            if (!_gamepadOnThumbstickMovedActions.ContainsKey(button))
            {
                _gamepadOnThumbstickMovedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _gamepadOnThumbstickMovedActions[button].Add(action);
            }
        }

        public void MapActionToGamepadTriggerMoved(Buttons button, Action<EventArgs> action)
        {
            if (!_gamepadOnTriggerMovedActions.ContainsKey(button))
            {
                _gamepadOnTriggerMovedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _gamepadOnTriggerMovedActions[button].Add(action);
            }
        }

        private void OnGamepadButtonDown(object sender, GamePadEventArgs e)
        {
            if (_gamepadOnButtonDownActions.ContainsKey(e.Button))
            {
                _gamepadOnButtonDownActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        private void OnGamepadButtonUp(object sender, GamePadEventArgs e)
        {
            if (_gamepadOnButtonUpActions.ContainsKey(e.Button))
            {
                _gamepadOnButtonUpActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        private void OnGamepadButtonRepeated(object sender, GamePadEventArgs e)
        {
            if (_gamepadOnButtonRepeatedActions.ContainsKey(e.Button))
            {
                _gamepadOnButtonRepeatedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        private void OnGamepadThumbstickMoved(object sender, GamePadEventArgs e)
        {
            if (_gamepadOnThumbstickMovedActions.ContainsKey(e.Button))
            {
                _gamepadOnThumbstickMovedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        private void OnGamepadTriggerMoved(object sender, GamePadEventArgs e)
        {
            if (_gamepadOnTriggerMovedActions.ContainsKey(e.Button))
            {
                _gamepadOnTriggerMovedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        #endregion
    }
}