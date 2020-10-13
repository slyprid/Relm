using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Relm.Input
{
    public class TouchManager
    {
        private TouchCollection _touches;

        public Vector2 Position
        {
            get
            {
                if (_touches.Count == 0) return new Vector2(-1, -1);
                var touch = _touches.First();
                return touch.Position;
            }
        }

        public void Update(GameTime gameTime)
        {
            _touches = TouchPanel.GetState();
        }

        public bool IsPressed(Rectangle bounds)
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            if (touch.State == TouchLocationState.Pressed)
            {
                return bounds.Contains(touch.Position);
            }

            return false;
        }

        public bool IsPressed()
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            return touch.State == TouchLocationState.Pressed;
        }

        public bool IsReleased(Rectangle bounds)
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            if (touch.State == TouchLocationState.Released)
            {
                return bounds.Contains(touch.Position);
            }

            return false;
        }

        public bool IsReleased()
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            return touch.State == TouchLocationState.Released;
        }

        public bool IsHeld(Rectangle bounds)
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            TouchLocation prevLoc;

            if (!touch.TryGetPreviousLocation(out prevLoc) || prevLoc.State != TouchLocationState.Moved)
                return false;

            if (touch.State != TouchLocationState.Released)
            {
                return bounds.Contains(touch.Position);
            }

            return false;
        }

        public bool IsHeld()
        {
            if (_touches.Count == 0) return false;

            var touch = _touches.First();

            TouchLocation prevLoc;

            if (!touch.TryGetPreviousLocation(out prevLoc) || prevLoc.State != TouchLocationState.Moved)
                return false;

            return touch.State != TouchLocationState.Released;
        }
    }
}