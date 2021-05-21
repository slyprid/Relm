using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class GamePadManager
    {
        private GamePadState _current;
        private GamePadState _previous;

        public void Update(GameTime gameTime)
        {
            _previous = _current;
            _current = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsButtonPressed(Buttons button)
        {
            return _current.IsButtonDown(button) && _previous.IsButtonUp(button);
        }

        public bool IsButtonHeld(Buttons button)
        {
            return _current.IsButtonDown(button) && _previous.IsButtonDown(button);
        }

        public bool IsButtonReleased(Buttons button)
        {
            return _current.IsButtonUp(button) && _previous.IsButtonDown(button);
        }
    }
}