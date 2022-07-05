using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Graphics;

namespace Relm.Input
{
    internal class MouseManager
    {
        private readonly MouseSettings _settings;
        private MouseState _currentState;
        private MouseState _previousState;
        private bool _dragging;
        private GameTime _gameTime;
        private bool _hasDoubleClicked;
        private MouseEventArgs _mouseDownArgs;
        private MouseEventArgs _previousClickArgs;

        public ViewportAdapter ViewportAdapter => _settings.ViewportAdapter;
        public int DoubleClickSpeed => _settings.DoubleClickSpeed;
        public int DragThreshold => _settings.DragThreshold;

        public bool HasMouseMoved => (_previousState.X != _currentState.X) || (_previousState.Y != _currentState.Y);

        public event EventHandler<MouseEventArgs> MouseDown;
        public event EventHandler<MouseEventArgs> MouseUp;
        public event EventHandler<MouseEventArgs> MouseClicked;
        public event EventHandler<MouseEventArgs> MouseDoubleClicked;
        public event EventHandler<MouseEventArgs> MouseMoved;
        public event EventHandler<MouseEventArgs> MouseWheelMoved;
        public event EventHandler<MouseEventArgs> MouseDragStart;
        public event EventHandler<MouseEventArgs> MouseDrag;
        public event EventHandler<MouseEventArgs> MouseDragEnd;

        public MouseManager() : this(new MouseSettings()) { }

        public MouseManager(MouseSettings settings)
        {
            _settings = settings;
        }

        public MouseManager(ViewportAdapter adapter) 
            : this(new MouseSettings())
        {
            _settings.ViewportAdapter = adapter;
        }

        public void Update(GameTime gameTime)
        {
            _gameTime = gameTime;
            _currentState = Mouse.GetState();

            CheckButtonPressed(s => s.LeftButton, MouseButton.Left);
            CheckButtonPressed(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonPressed(s => s.RightButton, MouseButton.Right);
            CheckButtonPressed(s => s.XButton1, MouseButton.XButton1);
            CheckButtonPressed(s => s.XButton2, MouseButton.XButton2);

            CheckButtonReleased(s => s.LeftButton, MouseButton.Left);
            CheckButtonReleased(s => s.MiddleButton, MouseButton.Middle);
            CheckButtonReleased(s => s.RightButton, MouseButton.Right);
            CheckButtonReleased(s => s.XButton1, MouseButton.XButton1);
            CheckButtonReleased(s => s.XButton2, MouseButton.XButton2);

            if (HasMouseMoved)
            {
                MouseMoved?.Invoke(this, new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));

                CheckMouseDragged(s => s.LeftButton, MouseButton.Left);
                CheckMouseDragged(s => s.MiddleButton, MouseButton.Middle);
                CheckMouseDragged(s => s.RightButton, MouseButton.Right);
                CheckMouseDragged(s => s.XButton1, MouseButton.XButton1);
                CheckMouseDragged(s => s.XButton2, MouseButton.XButton2);
            }

            if (_previousState.ScrollWheelValue != _currentState.ScrollWheelValue)
            {
                MouseWheelMoved?.Invoke(this, new MouseEventArgs(ViewportAdapter, gameTime.TotalGameTime, _previousState, _currentState));
            }

            _previousState = _currentState;
        }

        private void CheckButtonPressed(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) != ButtonState.Pressed) || (getButtonState(_previousState) != ButtonState.Released)) return;
            var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

            MouseDown?.Invoke(this, args);
            _mouseDownArgs = args;

            if (_previousClickArgs == null) return;
            var clickMilliseconds = (args.Time - _previousClickArgs.Time).TotalMilliseconds;

            if (clickMilliseconds <= DoubleClickSpeed)
            {
                MouseDoubleClicked?.Invoke(this, args);
                _hasDoubleClicked = true;
            }

            _previousClickArgs = null;
        }

        private void CheckButtonReleased(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) != ButtonState.Released) || (getButtonState(_previousState) != ButtonState.Pressed)) return;
            var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

            if (_mouseDownArgs.Button == args.Button)
            {
                var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                if (clickMovement < DragThreshold)
                {
                    if (!_hasDoubleClicked)
                        MouseClicked?.Invoke(this, args);
                }
                else 
                {
                    MouseDragEnd?.Invoke(this, args);
                    _dragging = false;
                }
            }

            MouseUp?.Invoke(this, args);

            _hasDoubleClicked = false;
            _previousClickArgs = args;
        }

        private void CheckMouseDragged(Func<MouseState, ButtonState> getButtonState, MouseButton button)
        {
            if ((getButtonState(_currentState) != ButtonState.Pressed) || (getButtonState(_previousState) != ButtonState.Pressed)) return;
            var args = new MouseEventArgs(ViewportAdapter, _gameTime.TotalGameTime, _previousState, _currentState, button);

            if (_mouseDownArgs.Button != args.Button) return;
            if (_dragging)
                MouseDrag?.Invoke(this, args);
            else
            {
                var clickMovement = DistanceBetween(args.Position, _mouseDownArgs.Position);

                if (clickMovement <= DragThreshold) return;
                _dragging = true;
                MouseDragStart?.Invoke(this, args);
            }
        }

        private static int DistanceBetween(Point a, Point b)
        {
            return System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y);
        }
    }
}