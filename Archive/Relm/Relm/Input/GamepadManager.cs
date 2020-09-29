using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Interfaces;

namespace Relm.Input
{
    public class GamepadManager
        : IUpdate
    {
        private readonly List<ButtonMap> _buttonMap = new List<ButtonMap>();

        public bool IsEnabled { get; set; }

        private GamePadState _previous;
        private GamePadState _current;

        public GamepadManager()
        {
            IsEnabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;

            _previous = _current;
            _current = GamePad.GetState(PlayerIndex.One);

            foreach (var map in _buttonMap)
            {
                switch (map.InputAction)
                {
                    case InputAction.Pressed:
                        if (IsButtonPressed(map.Button)) map.Action();
                        break;
                    case InputAction.Held:
                        if (IsButtonHeld(map.Button)) map.Action();
                        break;
                }
            }
        }

        public void Bind(Buttons button, InputAction inputAction, Action action)
        {
            _buttonMap.Add(new ButtonMap(button, inputAction, action));
        }

        public void Unbind(Buttons button)
        {
            var indexes = new List<int>();
            for (var i = 0; i < _buttonMap.Count; i++)
            {
                if (_buttonMap[i].Button == button) indexes.Add(i);
            }

            foreach (var idx in indexes)
            {
                _buttonMap.RemoveAt(idx);
            }
        }

        public bool IsButtonPressed(GamePadButtons button)
        {
            return _current.Buttons == button && _previous.Buttons != button;
        }

        public bool IsButtonPressed(Buttons button)
        {
            return _current.IsButtonDown(button) && _previous.IsButtonUp(button);
        }

        public bool IsButtonHeld(GamePadButtons button)
        {
            return _current.Buttons == button && _previous.Buttons == button;
        }

        public bool IsButtonHeld(Buttons button)
        {
            return _current.IsButtonDown(button) && _previous.IsButtonDown(button);
        }

        public bool IsAnyButtonPressed()
        {
            var ret = _current.IsButtonDown(Buttons.DPadUp)
                || _current.IsButtonDown(Buttons.DPadDown)
                || _current.IsButtonDown(Buttons.DPadLeft)
                || _current.IsButtonDown(Buttons.DPadRight)
                || _current.IsButtonDown(Buttons.Start)
                || _current.IsButtonDown(Buttons.Back)
                || _current.IsButtonDown(Buttons.LeftStick)
                || _current.IsButtonDown(Buttons.RightStick)
                || _current.IsButtonDown(Buttons.LeftShoulder)
                || _current.IsButtonDown(Buttons.RightShoulder)
                || _current.IsButtonDown(Buttons.A)
                || _current.IsButtonDown(Buttons.B)
                || _current.IsButtonDown(Buttons.X)
                || _current.IsButtonDown(Buttons.Y)
                || _current.IsButtonDown(Buttons.LeftThumbstickLeft)
                || _current.IsButtonDown(Buttons.RightTrigger)
                || _current.IsButtonDown(Buttons.LeftTrigger)
                || _current.IsButtonDown(Buttons.RightThumbstickUp)
                || _current.IsButtonDown(Buttons.RightThumbstickDown)
                || _current.IsButtonDown(Buttons.RightThumbstickRight)
                || _current.IsButtonDown(Buttons.RightThumbstickLeft)
                || _current.IsButtonDown(Buttons.LeftThumbstickUp)
                || _current.IsButtonDown(Buttons.LeftThumbstickDown)
                || _current.IsButtonDown(Buttons.LeftThumbstickRight);

            return ret;
        }

        public bool IsButtonReleased(Buttons button)
        {
            return _current.IsButtonUp(button) && _previous.IsButtonDown(button);
        }
    }
}