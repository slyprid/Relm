using System;
using Microsoft.Xna.Framework;
using Relm.Input;

namespace Relm.Events
{
    public class InputEvent
        : Event
    {
        public InputManager Input => Relm.States.GameState.Input;

        public Func<InputManager, bool> InputCheck { get; set; }

        public override void Update(GameTime gameTime, double elapsed)
        {
            if (!IsEnabled) return;

            if (InputCheck == null) return;
            if (InputCheck(Input))
            {
                OnActivate?.Invoke(this, AttachedObject);
            }
        }
    }
}