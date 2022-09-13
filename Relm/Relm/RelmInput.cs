using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Collections;
using Relm.Events;
using Relm.Input;
using Relm.Math;

namespace Relm
{
	public static class RelmInput
	{
		public static Emitter<InputEventType, InputEvent> Emitter;

		public static GamePadData[] GamePads;
		public const float DEFAULT_DEADZONE = 0.1f;

		internal static Vector2 _resolutionScale;
		internal static Point _resolutionOffset;
		static KeyboardState _previousKbState;
		static KeyboardState _currentKbState;
		static MouseState _previousMouseState;
		static MouseState _currentMouseState;
		static internal FastList<VirtualInput> _virtualInputs = new FastList<VirtualInput>();
		static int _maxSupportedGamePads;

		public static TouchInput Touch;

		public static Vector2 ResolutionScale => _resolutionScale;

		public static Vector2 ResolutionOffset => _resolutionOffset.ToVector2();

		public static int MaxSupportedGamePads
		{
			get { return _maxSupportedGamePads; }
			set
			{
#if FNA
				_maxSupportedGamePads = Mathf.Clamp( value, 1, 8 );
#else
				_maxSupportedGamePads = Mathf.Clamp(value, 1, GamePad.MaximumGamePadCount);
#endif
				GamePads = new GamePadData[_maxSupportedGamePads];
				for (var i = 0; i < _maxSupportedGamePads; i++)
					GamePads[i] = new GamePadData((PlayerIndex)i);
			}
		}

        public static GamePadData Player1Controller => GamePads[0];
        public static GamePadData Player2Controller => GamePads[1];
        public static GamePadData Player3Controller => GamePads[2];
        public static GamePadData Player4Controller => GamePads[3];


		static RelmInput()
		{
			Emitter = new Emitter<InputEventType, InputEvent>();
			Touch = new TouchInput();

			_previousKbState = new KeyboardState();
			_currentKbState = Keyboard.GetState();

			_previousMouseState = new MouseState();
			_currentMouseState = Mouse.GetState();

			MaxSupportedGamePads = 1;
		}


		public static void Update()
		{
			Touch.Update();

			_previousKbState = _currentKbState;
			_currentKbState = Keyboard.GetState();

			_previousMouseState = _currentMouseState;
			_currentMouseState = Mouse.GetState();

			for (var i = 0; i < _maxSupportedGamePads; i++)
				GamePads[i].Update();

			for (var i = 0; i < _virtualInputs.Length; i++)
				_virtualInputs.Buffer[i].Update();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Vector2 ScaledPosition(Vector2 position)
		{
			var scaledPos = new Vector2(position.X - _resolutionOffset.X, position.Y - _resolutionOffset.Y);
			return scaledPos * _resolutionScale;
		}

		public static void SetCurrentMouseState(MouseState state)
		{
			_currentMouseState = state;
		}

		public static void SetPreviousMouseState(MouseState state)
		{
			_previousMouseState = state;
		}

		public static void SetCurrentKeyboardState(KeyboardState state)
		{
			_currentKbState = state;
		}

		public static void SetPreviousKeyboardState(KeyboardState state)
		{
			_previousKbState = state;
		}

		#region Keyboard

		public static KeyboardState PreviousKeyboardState => _previousKbState;

		public static KeyboardState CurrentKeyboardState => _currentKbState;


		public static bool IsKeyPressed(Keys key)
		{
			return _currentKbState.IsKeyDown(key) && !_previousKbState.IsKeyDown(key);
		}


		public static bool IsKeyDown(Keys key)
		{
			return _currentKbState.IsKeyDown(key);
		}


		public static bool IsKeyReleased(Keys key)
		{
			return !_currentKbState.IsKeyDown(key) && _previousKbState.IsKeyDown(key);
		}


		public static bool IsKeyPressed(Keys keyA, Keys keyB)
		{
			return IsKeyPressed(keyA) || IsKeyPressed(keyB);
		}


		public static bool IsKeyDown(Keys keyA, Keys keyB)
		{
			return IsKeyDown(keyA) || IsKeyDown(keyB);
		}


		public static bool IsKeyReleased(Keys keyA, Keys keyB)
		{
			return IsKeyReleased(keyA) || IsKeyReleased(keyB);
		}

        public static bool IsAnyKeyPressed()
        {
            return _currentKbState.GetPressedKeys().Length > 0 && _previousKbState.GetPressedKeys().Length == 0;
        }

		#endregion


		#region Mouse

		public static MouseState PreviousMouseState => _previousMouseState;

		public static MouseState CurrentMouseState => _currentMouseState;

		public static bool LeftMouseButtonPressed =>
			_currentMouseState.LeftButton == ButtonState.Pressed &&
			_previousMouseState.LeftButton == ButtonState.Released;

		public static bool LeftMouseButtonDown => _currentMouseState.LeftButton == ButtonState.Pressed;

		public static bool LeftMouseButtonReleased =>
			_currentMouseState.LeftButton == ButtonState.Released &&
			_previousMouseState.LeftButton == ButtonState.Pressed;

		public static bool RightMouseButtonPressed =>
			_currentMouseState.RightButton == ButtonState.Pressed &&
			_previousMouseState.RightButton == ButtonState.Released;

		public static bool RightMouseButtonDown => _currentMouseState.RightButton == ButtonState.Pressed;

		public static bool RightMouseButtonReleased =>
			_currentMouseState.RightButton == ButtonState.Released &&
			_previousMouseState.RightButton == ButtonState.Pressed;

		public static bool MiddleMouseButtonPressed =>
			_currentMouseState.MiddleButton == ButtonState.Pressed &&
			_previousMouseState.MiddleButton == ButtonState.Released;

		public static bool MiddleMouseButtonDown => _currentMouseState.MiddleButton == ButtonState.Pressed;

		public static bool MiddleMouseButtonReleased =>
			_currentMouseState.MiddleButton == ButtonState.Released &&
			_previousMouseState.MiddleButton == ButtonState.Pressed;

		public static bool FirstExtendedMouseButtonPressed =>
			_currentMouseState.XButton1 == ButtonState.Pressed &&
			_previousMouseState.XButton1 == ButtonState.Released;

		public static bool FirstExtendedMouseButtonDown => _currentMouseState.XButton1 == ButtonState.Pressed;

		public static bool FirstExtendedMouseButtonReleased =>
			_currentMouseState.XButton1 == ButtonState.Released &&
			_previousMouseState.XButton1 == ButtonState.Pressed;

		public static bool SecondExtendedMouseButtonPressed =>
			_currentMouseState.XButton2 == ButtonState.Pressed &&
			_previousMouseState.XButton2 == ButtonState.Released;

		public static bool SecondExtendedMouseButtonDown => _currentMouseState.XButton2 == ButtonState.Pressed;

		public static bool SecondExtendedMouseButtonReleased =>
			_currentMouseState.XButton2 == ButtonState.Released &&
			_previousMouseState.XButton2 == ButtonState.Pressed;

		public static int MouseWheel => _currentMouseState.ScrollWheelValue;

		public static int MouseWheelDelta => _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;

		public static Point RawMousePosition => new Point(_currentMouseState.X, _currentMouseState.Y);

		public static Vector2 MousePosition => ScaledMousePosition;
        public static RectangleF MouseBounds => new(MousePosition, Vector2.One);

		public static Vector2 ScaledMousePosition => ScaledPosition(new Vector2(_currentMouseState.X, _currentMouseState.Y));

		public static Point MousePositionDelta =>
			new Point(_currentMouseState.X, _currentMouseState.Y) -
			new Point(_previousMouseState.X, _previousMouseState.Y);

		public static Vector2 ScaledMousePositionDelta
		{
			get
			{
				var pastPos = new Vector2(_previousMouseState.X - _resolutionOffset.X,
					_previousMouseState.Y - _resolutionOffset.Y);
				pastPos *= _resolutionScale;
				return ScaledMousePosition - pastPos;
			}
		}

		#endregion
    }
}