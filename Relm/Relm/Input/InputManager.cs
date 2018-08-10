using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Interfaces;

namespace Relm.Input
{
    public class InputManager
        : IUpdate
    {
        public bool IsEnabled { get; set; }

        public KeyboardManager Keyboard { get; }
        public GamepadManager Gamepad { get; }

        public InputManager()
        {
            Keyboard = new KeyboardManager();
            Gamepad = new GamepadManager();
            IsEnabled = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;

            Keyboard.Update(gameTime);
            Gamepad.Update(gameTime);
        }

        public void BindKey(Keys key, InputAction inputAction, Action action)
        {
            Keyboard.Bind(key, inputAction, action);
        }

        public void BindButton(Buttons button, InputAction inputAction, Action action)
        {
            Gamepad.Bind(button, inputAction, action);
        }

        public void UnbindKey(Keys key)
        {
            Keyboard.Unbind(key);
        }

        public void UnbindButton(Buttons button)
        {
            Gamepad.Unbind(button);
        }
    }
}