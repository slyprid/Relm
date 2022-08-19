using System;
using Relm.Core;

namespace Relm.Timers
{
    public class Timer : ITimer
    {
        private float _timeInSeconds;
        private bool _repeats;
        private Action<ITimer> _onTime;
        private bool _isDone;
        private float _elapsedTime;

        public object Context { get; set; }
        
        public void Stop()
        {
            _isDone = true;
        }

        public void Reset()
        {
            _elapsedTime = 0f;
        }

        public T GetContext<T>()
        {
            return (T)Context;
        }

        internal bool Tick()
        {
            if (!_isDone && _elapsedTime > _timeInSeconds)
            {
                _elapsedTime -= _timeInSeconds;
                _onTime(this);

                if (!_isDone && !_repeats) _isDone = true;
            }

            _elapsedTime += Time.DeltaTime;

            return _isDone;
        }

        internal void Initialize(float timeInSeconds, bool repeats, object context, Action<ITimer> onTime)
        {
            _timeInSeconds = timeInSeconds;
            _repeats = repeats;
            Context = context;
            _onTime = onTime;
        }

        internal void Unload()
        {
            Context = null;
            _onTime = null;
        }
    }
}