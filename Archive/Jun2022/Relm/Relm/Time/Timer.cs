using System;
using Microsoft.Xna.Framework;

namespace Relm.Time
{
    public class Timer
    {
        public string Key { get; set; }

        /// <summary>
        /// Time in milliseconds (ms)
        /// </summary>
        public int Duration { get; set; }

        public Action OnComplete { get; set; }

        public double Elapsed { get; set; }

        public bool IsComplete { get; set; }

        public Timer() { }

        public Timer(int duration, Action onComplete)
        {
            Duration = duration;
            OnComplete = onComplete;
        }

        public void Update(GameTime gameTime)
        {
            if (IsComplete) return;
            Elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Elapsed >= Duration)
            {
                OnComplete?.Invoke();
                IsComplete = true;
            }
        }
    }
}