using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace Relm.Events
{
    public class EventManager
    {
        public Dictionary<string, Event> Events { get; set; }

        public double Elapsed { get; private set; }

        public Event this[string name] => Events[name];

        public EventManager()
        {
            Events = new Dictionary<string, Event>();
        }

        public void Add(Event evt)
        {
            if (Events.ContainsKey(evt.Name))
            {
                Debug.WriteLine("This event has already been added", "WARNING");
                return;
            }
            Events.Add(evt.Name, evt);
        }

        public void Remove(Event evt)
        {
            Events.Remove(evt.Name);
        }

        public void Clear()
        {
            Events.Clear();
        }

        public void Update(GameTime gameTime)
        {
            Elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            foreach (var evt in Events.Values)
            {
                evt.Update(gameTime, Elapsed);
            }
        }
    }
}