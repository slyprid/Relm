using System;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class KeyMap
    {
        public Keys Key { get; set; }
        public InputAction InputAction { get; set; }
        public Action Action { get; set; }

        public KeyMap(Keys key, InputAction inputAction, Action action)
        {
            Key = key;
            InputAction = inputAction;
            Action = action;
        }
    }
}