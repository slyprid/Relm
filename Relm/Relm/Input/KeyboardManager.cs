using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class KeyboardManager
    {
        private KeyboardState _previousState;
        private KeyboardState _currentState;

        public void Update(GameTime gameTime)
        {
            _previousState = _currentState;
            _currentState = Keyboard.GetState();
        }

        public bool IsKeyPressed(Keys key)
        {
            return _currentState.IsKeyDown(key) && _previousState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return _currentState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return _currentState.IsKeyUp(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return _currentState.IsKeyUp(key) && _previousState.IsKeyDown(key);
        }

        public bool IsKeyHeld(Keys key)
        {
            return _currentState.IsKeyDown(key) && _previousState.IsKeyDown(key);
        }
    }
}