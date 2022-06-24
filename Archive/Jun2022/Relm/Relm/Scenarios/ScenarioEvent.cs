using Microsoft.Xna.Framework;

namespace Relm
{
    public abstract class ScenarioEvent
    {
        public abstract string Name { get; }
        public bool IsRequired { get; set; }
        public bool IsComplete { get; set; }
        
        public abstract void Update(GameTime gameTime);

        public virtual void OnLoad() { }
    }
}