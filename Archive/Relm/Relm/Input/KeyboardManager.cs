using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Interfaces;

namespace Relm.Input
{
    public class KeyboardManager
        : IUpdate
    {
        private readonly List<KeyMap> _keyMap = new List<KeyMap>();

        public bool IsEnabled { get; set; }

        private KeyboardState _previous;
        private KeyboardState _current;

        public KeyboardManager()
        {
            IsEnabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;

            _previous = _current;
            _current = Keyboard.GetState();

            foreach (var map in _keyMap)
            {
                switch (map.InputAction)
                {
                    case InputAction.Pressed:
                        if (IsKeyPressed(map.Key)) map.Action();
                        break;
                    case InputAction.Held:
                        if (IsKeyHeld(map.Key)) map.Action();
                        break;
                }
            }
        }

        public void Bind(Keys key, InputAction inputAction, Action action)
        {
            _keyMap.Add(new KeyMap(key, inputAction, action));
        }

        public void Unbind(Keys key)
        {
            var indexes = new List<int>();
            for (var i = 0; i < _keyMap.Count; i++)
            {
                if (_keyMap[i].Key == key) indexes.Add(i);
            }

            foreach (var idx in indexes)
            {
                _keyMap.RemoveAt(idx);
            }
        }

        public bool IsKeyPressed(Keys key)
        {
            return _current.IsKeyDown(key) && _previous.IsKeyUp(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return _current.IsKeyDown(key) && _previous.IsKeyDown(key);
        }

        public bool IsAnyKeyPressed()
        {
            return _current.GetPressedKeys().Length > 0;
        }

        public bool IsKeyReleased(Keys key)
        {
            return _current.IsKeyUp(key) && _previous.IsKeyDown(key);
        }
    }
}