using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Relm
{
    public abstract class Scenario
    {
        public abstract string Name { get; }
        public ScenarioManager ScenarioManager { get; internal set; }
        public List<ScenarioEvent> Events { get; set; }

        public List<ScenarioEvent> ActiveEvents { get; set; }

        public bool IsComplete
        {
            get
            {
                var requiredCount = Events.Count(x => x.IsRequired);
                var completeCount = Events.Count(x => x.IsRequired && x.IsComplete);
                return requiredCount == completeCount;
            }
        }

        protected Scenario()
        {
            Events = new List<ScenarioEvent>();
            ActiveEvents = new List<ScenarioEvent>();
        }

        public virtual void Dispose() { }

        public virtual void Initialize() { }

        public virtual void LoadContent() { }

        public virtual void UnloadContent() { }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var scenarioEvent in ActiveEvents)
            {
                scenarioEvent.Update(gameTime);
            }
        }

        public void AddEvent(ScenarioEvent scenarioEvent)
        {
            Events.Add(scenarioEvent);
        }
    }
}