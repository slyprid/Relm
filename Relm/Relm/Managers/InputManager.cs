using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Graphics;
using Relm.Input;

namespace Relm.Managers
{
    public class InputManager
        : Manager
    {
        private readonly KeyboardManager _keyboardManager;
        private readonly GamePadManager _gamepadManager;
        private readonly MouseManager _mouseManager;

        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyPressedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyReleasedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyTypedActions;
        private readonly Dictionary<Keys, List<Action<EventArgs>>> _keyboardOnKeyDownActions;

        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonDownActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonUpActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnButtonRepeatedActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnThumbstickMovedActions;
        private readonly Dictionary<Buttons, List<Action<EventArgs>>> _gamepadOnTriggerMovedActions;

        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnButtonDownActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnButtonUpActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnButtonClickedActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnButtonDoubleClickedActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnMovedActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnWheelMovedActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnDragStartActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnDragActions;
        private readonly Dictionary<MouseButton, List<Action<EventArgs>>> _mouseOnDragEndActions;

        public InputManager(ViewportAdapter viewportAdapter)
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

            _mouseManager = new MouseManager(viewportAdapter);

            _mouseManager.MouseDown += OnMouseButtonDown;
            _mouseManager.MouseUp += OnMouseButtonUp;
            _mouseManager.MouseClicked += OnMouseClicked;
            _mouseManager.MouseDoubleClicked += OnMouseDoubleClicked;
            _mouseManager.MouseMoved += OnMouseMoved;
            _mouseManager.MouseWheelMoved += OnMouseWheelMoved;
            _mouseManager.MouseDragStart += OnMouseDragStart;
            _mouseManager.MouseDrag += OnMouseDrag;
            _mouseManager.MouseDragEnd += OnMouseDragEnd;

            _mouseOnButtonDownActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnButtonUpActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnButtonClickedActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnButtonDoubleClickedActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnMovedActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnWheelMovedActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnDragStartActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnDragActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
            _mouseOnDragEndActions = new Dictionary<MouseButton, List<Action<EventArgs>>>();
        }
        
        public override void Update(GameTime gameTime)
        {
            _keyboardManager.Update(gameTime);
            _gamepadManager.Update(gameTime);
            _mouseManager.Update(gameTime);
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

        public void ClearRegisteredGamepadActions()
        {
            _gamepadOnButtonDownActions.Clear();
            _gamepadOnButtonUpActions.Clear();
            _gamepadOnButtonRepeatedActions.Clear();
            _gamepadOnThumbstickMovedActions.Clear();
            _gamepadOnTriggerMovedActions.Clear();
        }

        #endregion

        #region Mouse Manager

        public void MapActionToMouseButtonDown(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnButtonDownActions.ContainsKey(button))
            {
                _mouseOnButtonDownActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnButtonDownActions[button].Add(action);
            }
        }

        private void OnMouseButtonDown(object sender, MouseEventArgs e)
        {
            if (_mouseOnButtonDownActions.ContainsKey(e.Button))
            {
                _mouseOnButtonDownActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseButtonUp(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnButtonUpActions.ContainsKey(button))
            {
                _mouseOnButtonUpActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnButtonUpActions[button].Add(action);
            }
        }

        private void OnMouseButtonUp(object sender, MouseEventArgs e)
        {
            if (_mouseOnButtonUpActions.ContainsKey(e.Button))
            {
                _mouseOnButtonUpActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseButtonClicked(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnButtonClickedActions.ContainsKey(button))
            {
                _mouseOnButtonClickedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnButtonClickedActions[button].Add(action);
            }
        }

        private void OnMouseClicked(object sender, MouseEventArgs e)
        {
            if (_mouseOnButtonClickedActions.ContainsKey(e.Button))
            {
                _mouseOnButtonClickedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseButtonDoubleClicked(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnButtonDoubleClickedActions.ContainsKey(button))
            {
                _mouseOnButtonDoubleClickedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnButtonDoubleClickedActions[button].Add(action);
            }
        }

        private void OnMouseDoubleClicked(object sender, MouseEventArgs e)
        {
            if (_mouseOnButtonDoubleClickedActions.ContainsKey(e.Button))
            {
                _mouseOnButtonDoubleClickedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseMoved(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnMovedActions.ContainsKey(button))
            {
                _mouseOnMovedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnMovedActions[button].Add(action);
            }
        }

        private void OnMouseMoved(object sender, MouseEventArgs e)
        {
            if (_mouseOnMovedActions.ContainsKey(e.Button))
            {
                _mouseOnMovedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseWheelMoved(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnWheelMovedActions.ContainsKey(button))
            {
                _mouseOnWheelMovedActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnWheelMovedActions[button].Add(action);
            }
        }

        private void OnMouseWheelMoved(object sender, MouseEventArgs e)
        {
            if (_mouseOnWheelMovedActions.ContainsKey(e.Button))
            {
                _mouseOnWheelMovedActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseDragStart(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnDragStartActions.ContainsKey(button))
            {
                _mouseOnDragStartActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnDragStartActions[button].Add(action);
            }
        }

        private void OnMouseDragStart(object sender, MouseEventArgs e)
        {
            if (_mouseOnDragStartActions.ContainsKey(e.Button))
            {
                _mouseOnDragStartActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseDrag(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnDragActions.ContainsKey(button))
            {
                _mouseOnDragActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnDragActions[button].Add(action);
            }
        }

        private void OnMouseDrag(object sender, MouseEventArgs e)
        {
            if (_mouseOnDragActions.ContainsKey(e.Button))
            {
                _mouseOnDragActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void MapActionToMouseDragEnd(MouseButton button, Action<EventArgs> action)
        {
            if (!_mouseOnDragEndActions.ContainsKey(button))
            {
                _mouseOnDragEndActions.Add(button, new List<Action<EventArgs>> { action });
            }
            else
            {
                _mouseOnDragEndActions[button].Add(action);
            }
        }

        private void OnMouseDragEnd(object sender, MouseEventArgs e)
        {
            if (_mouseOnDragEndActions.ContainsKey(e.Button))
            {
                _mouseOnDragEndActions[e.Button].ForEach(action => action.Invoke(e));
            }
        }

        public void ClearRegisteredMouseActions()
        {
            _mouseOnButtonDownActions.Clear();
            _mouseOnButtonUpActions.Clear();
            _mouseOnButtonClickedActions.Clear();
            _mouseOnButtonDoubleClickedActions.Clear();
            _mouseOnMovedActions.Clear();
            _mouseOnWheelMovedActions.Clear();
            _mouseOnDragStartActions.Clear();
            _mouseOnDragActions.Clear();
            _mouseOnDragEndActions.Clear();
        }

        #endregion
    }
}