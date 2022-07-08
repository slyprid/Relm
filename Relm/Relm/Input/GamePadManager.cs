using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    internal class GamePadManager
    {
        private static readonly bool[] GamePadConnections = new bool[4];

        private readonly Buttons[] _excludedButtons =
        {
            Buttons.LeftTrigger, Buttons.RightTrigger,
            Buttons.LeftThumbstickDown, Buttons.LeftThumbstickUp, Buttons.LeftThumbstickRight,
            Buttons.LeftThumbstickLeft,
            Buttons.RightThumbstickLeft, Buttons.RightThumbstickRight, Buttons.RightThumbstickUp,
            Buttons.RightThumbstickDown
        };

        private GamePadSettings _settings;
        private GameTime _gameTime;
        private GameTime _previousGameTime;
        private GamePadState _currentState;
        private GamePadState _previousState;
        private GamePadState _lastTriggerState;
        private GamePadState _lastThumbStickState;

        private bool _leftVibrating;
        private float _leftCurVibrationStrength;
        private float _vibrationStrengthLeft;
        private TimeSpan _vibrationDurationLeft;
        private bool _leftTriggerDown;
        private bool _leftStickDown;
        private Buttons _lastLeftStickDirection;

        private bool _rightVibrating;
        private float _rightCurVibrationStrength;
        private float _vibrationStrengthRight;
        private TimeSpan _vibrationDurationRight;
        private bool _rightTriggerDown;
        private bool _rightStickDown;
        private Buttons _lastRightStickDirection;

        private TimeSpan _vibrationStart;
        private Buttons _lastButton;
        private int _repeatedButtonTimer;

        public static bool CheckControllerConnections { get; set; }

        public PlayerIndex PlayerIndex => _settings.PlayerIndex;
        public float TriggerDownThreshold => _settings.TriggerDownThreshold;
        public float TriggerDeltaThreshold => _settings.TriggerDeltaThreshold;
        public float ThumbstickDownThreshold => _settings.ThumbstickDownThreshold;
        public float ThumbstickDeltaThreshold => _settings.ThumbstickDownThreshold;

        public bool VibrationEnabled { get; set; }
        public int RepeatInitialDelay { get; }
        public int RepeatDelay { get; }
        public bool IsConnected { get; private set; }

        public float VibrationStrengthLeft
        {
            get => _vibrationStrengthLeft;
            set => _vibrationStrengthLeft = MathHelper.Clamp(value, 0, 1);
        }

        public float VibrationStrengthRight
        {
            get => _vibrationStrengthRight;
            set => _vibrationStrengthRight = MathHelper.Clamp(value, 0, 1);
        }

        public event EventHandler<GamePadEventArgs> ButtonDown;
        public event EventHandler<GamePadEventArgs> ButtonUp;
        public event EventHandler<GamePadEventArgs> ButtonRepeated;
        public event EventHandler<GamePadEventArgs> ThumbStickMoved;
        public event EventHandler<GamePadEventArgs> TriggerMoved;
        public static event EventHandler<GamePadEventArgs> ControllerConnectionChanged;

        public GamePadManager() : this(new GamePadSettings()) { }

        public GamePadManager(GamePadSettings settings)
        {
            _settings = settings;
            VibrationEnabled = settings.VibrationEnabled;
            RepeatInitialDelay = settings.RepeatInitialDelay;
            RepeatDelay = settings.RepeatDelay;
            _previousGameTime = new GameTime();
            _previousState = GamePadState.Default;
        }

        public void Update(GameTime gameTime)
        {
            IsConnected = false;
            _gameTime = gameTime;
            _currentState = GamePad.GetState(PlayerIndex);
            CheckVibrate();
            if (!_currentState.IsConnected) return;
            IsConnected = true;
            CheckAllButtons();
            CheckRepeatButton();
            _previousGameTime = _gameTime;
            _previousState = _currentState;
        }

        #region Vibration

        private void CheckVibrate()
        {
            if (_leftVibrating && (_vibrationStart + _vibrationDurationLeft < _gameTime.TotalGameTime))
                Vibrate(0, 0);
            if (_rightVibrating && (_vibrationStart + _vibrationDurationRight < _gameTime.TotalGameTime))
                Vibrate(0, rightStrength: 0);
        }

        public bool Vibrate(int durationMs, float leftStrength = float.NegativeInfinity,
            float rightStrength = float.NegativeInfinity)
        {
            if (!VibrationEnabled) return false;

            var lstrength = MathHelper.Clamp(leftStrength, 0, 1);
            var rstrength = MathHelper.Clamp(rightStrength, 0, 1);

            if (float.IsNegativeInfinity(leftStrength))
                lstrength = _leftCurVibrationStrength;
            if (float.IsNegativeInfinity(rightStrength))
                rstrength = _rightCurVibrationStrength;

            var success = GamePad.SetVibration(PlayerIndex, lstrength * VibrationStrengthLeft, rstrength * VibrationStrengthRight);

            if (!success) return false;

            _leftVibrating = true;
            _rightVibrating = true;

            if (leftStrength > 0) _vibrationDurationLeft = new TimeSpan(0, 0, 0, 0, durationMs);
            else
            {
                if (lstrength > 0) _vibrationDurationLeft -= _gameTime.TotalGameTime - _vibrationStart;
                else _leftVibrating = false;
            }

            if (rightStrength > 0) _vibrationDurationRight = new TimeSpan(0, 0, 0, 0, durationMs);
            else
            {
                if (rstrength > 0) _vibrationDurationRight -= _gameTime.TotalGameTime - _vibrationStart;
                else _rightVibrating = false;
            }

            _vibrationStart = _gameTime.TotalGameTime;

            _leftCurVibrationStrength = lstrength;
            _rightCurVibrationStrength = rstrength;

            return true;
        }

        #endregion

        #region Buttons

        private void CheckAllButtons()
        {
            foreach (Buttons button in Enum.GetValues(typeof(Buttons)))
            {
                if (_excludedButtons.ToList().Contains(button))
                    break;
                if (_currentState.IsButtonDown(button) && _previousState.IsButtonUp(button))
                    RaiseButtonDown(button);
                if (_currentState.IsButtonUp(button) && _previousState.IsButtonDown(button))
                    RaiseButtonUp(button);
            }

            CheckTriggers(s => s.Triggers.Left, Buttons.LeftTrigger);
            CheckTriggers(s => s.Triggers.Right, Buttons.RightTrigger);

            CheckThumbSticks(s => s.ThumbSticks.Right, Buttons.RightStick);
            CheckThumbSticks(s => s.ThumbSticks.Left, Buttons.LeftStick);
        }

        private void RaiseButtonDown(Buttons button)
        {
            ButtonDown?.Invoke(this, MakeArgs(button));
            ButtonRepeated?.Invoke(this, MakeArgs(button));
            _lastButton = button;
            _repeatedButtonTimer = 0;
        }

        private void RaiseButtonUp(Buttons button)
        {
            ButtonUp?.Invoke(this, MakeArgs(button));
            _lastButton = 0;
        }

        private void CheckTriggers(Func<GamePadState, float> getButtonState, Buttons button)
        {
            var debounce = 0.05f;
            var curstate = getButtonState(_currentState);
            var curdown = curstate > TriggerDownThreshold;
            var prevdown = button == Buttons.RightTrigger ? _rightTriggerDown : _leftTriggerDown;

            if (!prevdown && curdown)
            {
                RaiseButtonDown(button);
                if (button == Buttons.RightTrigger)
                    _rightTriggerDown = true;
                else
                    _leftTriggerDown = true;
            }
            else
            {
                if (prevdown && (curstate < debounce))
                {
                    RaiseButtonUp(button);
                    if (button == Buttons.RightTrigger)
                        _rightTriggerDown = false;
                    else
                        _leftTriggerDown = false;
                }
            }

            var prevstate = getButtonState(_lastTriggerState);
            if (curstate > TriggerDeltaThreshold)
            {
                if (System.Math.Abs(prevstate - curstate) >= TriggerDeltaThreshold)
                {
                    TriggerMoved?.Invoke(this, MakeArgs(button, curstate));
                    _lastTriggerState = _currentState;
                }
            }
            else
            {
                if (prevstate > TriggerDeltaThreshold)
                {
                    TriggerMoved?.Invoke(this, MakeArgs(button, curstate));
                    _lastTriggerState = _currentState;
                }
            }
        }

        private void CheckThumbSticks(Func<GamePadState, Vector2> getButtonState, Buttons button)
        {
            const float debounce = 0.15f;
            var curVector = getButtonState(_currentState);
            var curdown = curVector.Length() > ThumbstickDownThreshold;
            var right = button == Buttons.RightStick;
            var prevdown = right ? _rightStickDown : _leftStickDown;

            var prevdir = button == Buttons.RightStick ? _lastRightStickDirection : _lastLeftStickDirection;
            GamePadThumbSticks? thumbstick = null;
            Buttons curdir;
            if (curVector.Y > curVector.X)
            {
                if (curVector.Y > -curVector.X)
                    curdir = right ? Buttons.RightThumbstickUp : Buttons.LeftThumbstickUp;
                else
                    curdir = right ? Buttons.RightThumbstickLeft : Buttons.LeftThumbstickLeft;
            }
            else
            {
                if (curVector.Y < -curVector.X)
                    curdir = right ? Buttons.RightThumbstickDown : Buttons.LeftThumbstickDown;
                else
                    curdir = right ? Buttons.RightThumbstickRight : Buttons.LeftThumbstickRight;
            }

            if (!prevdown && curdown)
            {
                if (right)
                    _lastRightStickDirection = curdir;
                else
                    _lastLeftStickDirection = curdir;

                RaiseButtonDown(curdir);
                if (button == Buttons.RightStick)
                    _rightStickDown = true;
                else
                    _leftStickDown = true;
            }
            else
            {
                if (prevdown && (curVector.Length() < debounce))
                {
                    RaiseButtonUp(prevdir);
                    if (button == Buttons.RightStick)
                        _rightStickDown = false;
                    else
                        _leftStickDown = false;
                }
                else
                {
                    if (prevdown && curdown && (curdir != prevdir))
                    {
                        RaiseButtonUp(prevdir);
                        if (right)
                            _lastRightStickDirection = curdir;
                        else
                            _lastLeftStickDirection = curdir;
                        RaiseButtonDown(curdir);
                    }
                }
            }

            var prevVector = getButtonState(_lastThumbStickState);
            if (curVector.Length() > ThumbstickDeltaThreshold)
            {
                if (!(Vector2.Distance(curVector, prevVector) >= ThumbstickDeltaThreshold)) return;
                ThumbStickMoved?.Invoke(this, MakeArgs(button, thumbStickState: curVector));
                _lastThumbStickState = _currentState;
            }
            else
            {
                if (!(prevVector.Length() > ThumbstickDeltaThreshold)) return;
                ThumbStickMoved?.Invoke(this, MakeArgs(button, thumbStickState: curVector));
                _lastThumbStickState = _currentState;
            }
        }

        private void CheckRepeatButton()
        {
            _repeatedButtonTimer += _gameTime.ElapsedGameTime.Milliseconds;

            if ((_repeatedButtonTimer < RepeatInitialDelay) || (_lastButton == 0))
                return;

            if (_repeatedButtonTimer < RepeatInitialDelay + RepeatDelay)
            {
                ButtonRepeated?.Invoke(this, MakeArgs(_lastButton));
                _repeatedButtonTimer = RepeatDelay + RepeatInitialDelay;
            }
            else
            {
                if (_repeatedButtonTimer > RepeatInitialDelay + RepeatDelay * 2)
                {
                    ButtonRepeated?.Invoke(this, MakeArgs(_lastButton));
                    _repeatedButtonTimer = RepeatDelay + RepeatInitialDelay;
                }
            }
        }

        #endregion

        #region Utility Methods / Functions

        private GamePadEventArgs MakeArgs(Buttons? button, float triggerState = 0, Vector2? thumbStickState = null)
        {
            var elapsedTime = _gameTime.TotalGameTime - _previousGameTime.TotalGameTime;
            return new GamePadEventArgs(_previousState, _currentState, elapsedTime, PlayerIndex, button, triggerState, thumbStickState);
        }

        internal static void CheckConnections()
        {
            if (!CheckControllerConnections) return;

            foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
            {
                if (!(GamePad.GetState(index).IsConnected ^ GamePadConnections[(int)index])) continue;
                GamePadConnections[(int)index] = !GamePadConnections[(int)index];
                ControllerConnectionChanged?.Invoke(null, new GamePadEventArgs(GamePadState.Default, GamePad.GetState(index), TimeSpan.Zero, index));
            }
        }

        #endregion
    }
}