using System;
using System.Collections.Generic;
using Relm.Time;

namespace Relm
{
    public static class Timers
    {
        internal static TimerManager TimerManager { get; set; }
        
        public static void Add(string key, int msDuration, Action onComplete)
        {
            if (TimerManager.Timers.ContainsKey(key)) return;

            var timer = new Timer(msDuration, onComplete)
            {
                Key = key
            };

            TimerManager.Timers.Add(key, timer);
        }
    }
}