using System;
using Microsoft.Xna.Framework;

namespace Relm.Events
{
    public class Event
    {
        /// <summary>
        /// Start time in milliseconds
        /// </summary>
        public double StartTime { get; set; }

        /// <summary>
        /// End time in milliseconds
        /// </summary>
        public double EndTime { get; set; }

        /// <summary>
        /// Amount of time between each tick event while active
        /// </summary>
        public double TickTime { get; set; }

        public string Name { get; set; }

        public bool IsEnabled { get; set; }
        
        public bool IsActive { get; set; }
        
        public bool IsRepeatable { get; set; }
        public double Elapsed { get; set; }

        public Action<Event, object> OnActivate { get; set; }
        public Action<Event, object> OnDeactivate { get; set; }
        public Action<Event, object> OnTick { get; set; }

        public object AttachedObject { get; set; }

        public Event()
        {
            Name = Guid.NewGuid().ToString();
            IsEnabled = true;
        }
        public Event(string name)
        {
            Name = name;
            IsEnabled = true;
        }

        public void Stop()
        {
            IsEnabled = false;
        }

        public void Start()
        {
            IsEnabled = true;
        }

        public void Restart()
        {
            Elapsed = 0;
            Start();
        }

        public virtual void Update(GameTime gameTime, double elapsed)
        {
            if (!IsEnabled) return;

            if (!IsRepeatable)
            {

                if (elapsed >= StartTime && !IsActive)
                {
                    IsActive = true;
                    OnActivate(this, AttachedObject);
                }

                if (elapsed >= StartTime && elapsed < EndTime && IsActive)
                {
                    Elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                    if (Elapsed >= TickTime)
                    {
                        Elapsed -= TickTime;
                        OnTick(this, AttachedObject);
                    }
                }

                if (elapsed >= EndTime && IsActive)
                {
                    IsActive = false;
                    OnDeactivate(this, AttachedObject);
                }
            }
            else
            {
                Elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

                if (Elapsed >= TickTime)
                {
                    Elapsed -= TickTime;
                    OnTick(this, AttachedObject);
                }
            }
        }

        
    }
}