using System;
using System.Collections.Generic;
using Relm.Core;

namespace Relm.Timers
{
    public class TimerManager 
        : GlobalManager
    {
        private readonly List<Timer> _timers = new ();
        
        public override void Update()
        {
            for (var i = _timers.Count - 1; i >= 0; i--)
            {
                if (!_timers[i].Tick()) continue;
                _timers[i].Unload();
                _timers.RemoveAt(i);
            }
        }

        internal ITimer Schedule(float timeInSeconds, bool repeats, object context, Action<ITimer> onTime)
        {
            var timer = new Timer();
            timer.Initialize(timeInSeconds, repeats, context, onTime);
            _timers.Add(timer);

            return timer;
        }
    }
}