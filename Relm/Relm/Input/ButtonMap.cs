using System;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class ButtonMap
    {
        public Buttons Button { get; set; }
        public InputAction InputAction { get; set; }
        public Action Action { get; set; }

        public ButtonMap(Buttons button, InputAction inputAction, Action action)
        {
            Button = button;
            InputAction = inputAction;
            Action = action;
        }
    }
}