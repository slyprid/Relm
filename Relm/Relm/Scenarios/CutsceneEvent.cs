using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm
{
    public class CutsceneEvent
        : ScenarioEvent 
    {
        public override string Name { get; }

        public override void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}