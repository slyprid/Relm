using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Relm.Time
{
    public class TimerManager
        : SimpleGameComponent
    {
        public Dictionary<string, Timer> Timers { get; set; }

        public TimerManager()
        {
            Timers = new Dictionary<string, Timer>();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var timer in Timers.Values)
            {
                timer.Update(gameTime);
            }

            var deadTimers = Timers.Values.Where(x => x.IsComplete);
            foreach (var timer in deadTimers)
            {
                Timers.Remove(timer.Key);
            }
        }

        public void Clear()
        {
            Timers.Clear();
        }
    }
}