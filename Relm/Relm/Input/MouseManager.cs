using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class MouseManager
    {
        private MouseState _current;
        private MouseState _previous;

        public int X => _current.X;
        public int Y => _current.Y;

        public Vector2 Position => new Vector2(X, Y);

        public void Update(GameTime gameTime)
        {
            _previous = _current;
            _current = Mouse.GetState();
        }

        public bool IsPressed(MouseButtons button, Rectangle bounds)
        {
            var isPressed = false;

            switch (button)
            {
                case MouseButtons.Left:
                    isPressed = _current.LeftButton == ButtonState.Pressed && _previous.LeftButton == ButtonState.Released;
                    break;
                case MouseButtons.Middle:
                    isPressed = _current.MiddleButton == ButtonState.Pressed && _previous.MiddleButton == ButtonState.Released;
                    break;
                case MouseButtons.Right:
                    isPressed = _current.RightButton == ButtonState.Pressed && _previous.RightButton == ButtonState.Released;
                    break;
            }

            if (!isPressed) return false;

            var pointRect = new Rectangle((int)Position.X, (int)Position.Y, 1, 1);

            return pointRect.Intersects(bounds);
        }

        public bool IsPressed()
        {
            if (_current.LeftButton == ButtonState.Pressed && _previous.LeftButton == ButtonState.Released) return true;
            if (_current.MiddleButton == ButtonState.Pressed && _previous.MiddleButton == ButtonState.Released) return true;
            if (_current.RightButton == ButtonState.Pressed && _previous.RightButton == ButtonState.Released) return true;
            return false;
        }

        public bool IsReleased(MouseButtons button, Rectangle bounds)
        {
            var isPressed = false;

            switch (button)
            {
                case MouseButtons.Left:
                    isPressed = _current.LeftButton == ButtonState.Released && _previous.LeftButton == ButtonState.Pressed;
                    break;
                case MouseButtons.Middle:
                    isPressed = _current.MiddleButton == ButtonState.Released && _previous.MiddleButton == ButtonState.Pressed;
                    break;
                case MouseButtons.Right:
                    isPressed = _current.RightButton == ButtonState.Released && _previous.RightButton == ButtonState.Pressed;
                    break;
            }

            if (!isPressed) return false;

            var pointRect = new Rectangle((int)Position.X, (int)Position.Y, 1, 1);
            return pointRect.Intersects(bounds);
        }

        public bool IsHeld(MouseButtons button, Rectangle bounds)
        {
            var isPressed = false;

            switch (button)
            {
                case MouseButtons.Left:
                    isPressed = _current.LeftButton == ButtonState.Pressed && _previous.LeftButton == ButtonState.Pressed;
                    break;
                case MouseButtons.Middle:
                    isPressed = _current.MiddleButton == ButtonState.Pressed && _previous.MiddleButton == ButtonState.Pressed;
                    break;
                case MouseButtons.Right:
                    isPressed = _current.RightButton == ButtonState.Pressed && _previous.RightButton == ButtonState.Pressed;
                    break;
            }

            if (!isPressed) return false;

            var pointRect = new Rectangle((int)Position.X, (int)Position.Y, 1, 1);
            return pointRect.Intersects(bounds);
        }
    }
}