using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace Relm
{
    public class ScenarioManager
        : SimpleGameComponent
    {
        private Scenario _activeScenario;
        
        public Scenario ActiveScenario => _activeScenario;

        public void LoadScenario(Scenario scenario)
        {
            _activeScenario?.UnloadContent();
            _activeScenario?.Dispose();
            scenario.ScenarioManager = this;
            scenario.Initialize();
            scenario.LoadContent();
            _activeScenario = scenario;
        }

        public override void Update(GameTime gameTime)
        {
            _activeScenario?.Update(gameTime);
        }
    }
}