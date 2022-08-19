using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Relm.Core;

namespace Relm.Input
{
	public class GamePadData
	{
		public bool IsLeftStickVerticalInverted = false;

		public bool IsRightStickVerticalInverted = false;

		public GamePadDeadZone DeadZone = GamePadDeadZone.IndependentAxes;

		PlayerIndex _playerIndex;
		GamePadState _previousState;
		GamePadState _currentState;
		float _rumbleTime;


		internal GamePadData(PlayerIndex playerIndex)
		{
			_playerIndex = playerIndex;
			_previousState = new GamePadState();
			_currentState = GamePad.GetState(_playerIndex);
		}


		public void Update()
		{
			_previousState = _currentState;
			_currentState = GamePad.GetState(_playerIndex, DeadZone);

			if (_previousState.IsConnected != _currentState.IsConnected)
			{
				var data = new InputEvent
				{
					GamePadIndex = (int)_playerIndex
				};
				RelmInput.Emitter.Emit(
					_currentState.IsConnected ? InputEventType.GamePadConnected : InputEventType.GamePadDisconnected,
					data);
			}

			if (_rumbleTime > 0f)
			{
				_rumbleTime -= Time.DeltaTime;
				if (_rumbleTime <= 0f)
					GamePad.SetVibration(_playerIndex, 0, 0);
			}
		}


		public void SetVibration(float left, float right, float duration)
		{
			_rumbleTime = duration;
			GamePad.SetVibration(_playerIndex, left, right);
		}


		public void StopVibration()
		{
			GamePad.SetVibration(_playerIndex, 0, 0);
			_rumbleTime = 0f;
		}


		public bool IsConnected()
		{
			return _currentState.IsConnected;
		}


		#region Buttons

		public bool IsButtonPressed(Buttons button)
		{
			return _currentState.IsButtonDown(button) && !_previousState.IsButtonDown(button);
		}


		public bool IsButtonDown(Buttons button)
		{
			return _currentState.IsButtonDown(button);
		}


		public bool IsButtonReleased(Buttons button)
		{
			return !_currentState.IsButtonDown(button) && _previousState.IsButtonDown(button);
		}

		#endregion


		#region Sticks

		public Vector2 GetLeftStick()
		{
			var res = _currentState.ThumbSticks.Left;

			if (IsLeftStickVerticalInverted)
				res.Y = -res.Y;

			return res;
		}


		public Vector2 GetLeftStick(float deadzone)
		{
			var res = _currentState.ThumbSticks.Left;

			if (res.LengthSquared() < deadzone * deadzone)
				res = Vector2.Zero;
			else if (IsLeftStickVerticalInverted)
				res.Y = -res.Y;

			return res;
		}


		public Vector2 GetRightStick()
		{
			var res = _currentState.ThumbSticks.Right;

			if (IsRightStickVerticalInverted)
				res.Y = -res.Y;

			return res;
		}


		public Vector2 GetRightStick(float deadzone)
		{
			var res = _currentState.ThumbSticks.Right;

			if (res.LengthSquared() < deadzone * deadzone)
				res = Vector2.Zero;
			else if (IsRightStickVerticalInverted)
				res.Y = -res.Y;

			return res;
		}

		#endregion


		#region Sticks as Buttons

		public bool IsLeftStickLeft(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.X < -deadzone;
		}


		public bool IsLeftStickLeftPressed(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.X < -deadzone && _previousState.ThumbSticks.Left.X > -deadzone;
		}


		public bool IsLeftStickRight(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.X > deadzone;
		}


		public bool IsLeftStickRightPressed(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.X > deadzone && _previousState.ThumbSticks.Left.X < deadzone;
		}


		public bool IsLeftStickUp(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.Y > deadzone;
		}


		public bool IsLeftStickUpPressed(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.Y > deadzone && _previousState.ThumbSticks.Left.Y < deadzone;
		}


		public bool IsLeftStickDown(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.Y < -deadzone;
		}


		public bool IsLeftStickDownPressed(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Left.Y < -deadzone && _previousState.ThumbSticks.Left.Y > -deadzone;
		}


		public bool IsRightStickLeft(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Right.X < -deadzone;
		}


		public bool IsRightStickRight(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Right.X > deadzone;
		}


		public bool IsRightStickUp(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Right.Y > deadzone;
		}


		public bool IsRightStickDown(float deadzone = RelmInput.DEFAULT_DEADZONE)
		{
			return _currentState.ThumbSticks.Right.Y < -deadzone;
		}

		#endregion


		#region Dpad

		public bool DpadLeftDown => _currentState.DPad.Left == ButtonState.Pressed;


		public bool DpadLeftPressed =>
			_currentState.DPad.Left == ButtonState.Pressed &&
			_previousState.DPad.Left == ButtonState.Released;


		public bool DpadLeftReleased =>
			_currentState.DPad.Left == ButtonState.Released &&
			_previousState.DPad.Left == ButtonState.Pressed;


		public bool DpadRightDown => _currentState.DPad.Right == ButtonState.Pressed;


		public bool DpadRightPressed =>
			_currentState.DPad.Right == ButtonState.Pressed &&
			_previousState.DPad.Right == ButtonState.Released;


		public bool DpadRightReleased =>
			_currentState.DPad.Right == ButtonState.Released &&
			_previousState.DPad.Right == ButtonState.Pressed;


		public bool DpadUpDown => _currentState.DPad.Up == ButtonState.Pressed;


		public bool DpadUpPressed => _currentState.DPad.Up == ButtonState.Pressed && _previousState.DPad.Up == ButtonState.Released;


		public bool DpadUpReleased => _currentState.DPad.Up == ButtonState.Released && _previousState.DPad.Up == ButtonState.Pressed;


		public bool DpadDownDown => _currentState.DPad.Down == ButtonState.Pressed;


		public bool DpadDownPressed =>
			_currentState.DPad.Down == ButtonState.Pressed &&
			_previousState.DPad.Down == ButtonState.Released;


		public bool DpadDownReleased =>
			_currentState.DPad.Down == ButtonState.Released &&
			_previousState.DPad.Down == ButtonState.Pressed;

		#endregion


		#region Triggers

		public float GetLeftTriggerRaw()
		{
			return _currentState.Triggers.Left;
		}


		public float GetRightTriggerRaw()
		{
			return _currentState.Triggers.Right;
		}


		public bool IsLeftTriggerDown(float threshold = 0.2f)
		{
			return _currentState.Triggers.Left > threshold;
		}


		public bool IsLeftTriggerPressed(float threshold = 0.2f)
		{
			return _currentState.Triggers.Left > threshold && _previousState.Triggers.Left < threshold;
		}


		public bool IsLeftTriggerReleased(float threshold = 0.2f)
		{
			return _currentState.Triggers.Left < threshold && _previousState.Triggers.Left > threshold;
		}


		public bool IsRightTriggerDown(float threshold = 0.2f)
		{
			return _currentState.Triggers.Right > threshold;
		}


		public bool IsRightTriggerPressed(float threshold = 0.2f)
		{
			return _currentState.Triggers.Right > threshold && _previousState.Triggers.Right < threshold;
		}


		public bool IsRightTriggerReleased(float threshold = 0.2f)
		{
			return _currentState.Triggers.Right < threshold && _previousState.Triggers.Right > threshold;
		}

		#endregion
	}
}