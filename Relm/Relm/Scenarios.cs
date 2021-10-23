using System.Collections.Generic;
using MonoGame.Extended.Screens.Transitions;
using Relm.UI;

namespace Relm
{
    public static class Scenarios
    {
        internal static ScenarioManager ScenarioManager { get; set; }

        public static Dictionary<string, Scenario> ScenarioList { get; }

        public static Scenario ActiveScenario => ScenarioManager.ActiveScenario;

        static Scenarios()
        {
            ScenarioList = new Dictionary<string, Scenario>();
        }

        public static bool IsActiveScreen(Scenario scenario)
        {
            return ActiveScenario == scenario;
        }

        public static void LoadScenario(Scenario scenario)
        {
            ScenarioManager.LoadScenario(scenario);
        }

        public static Scenario Add(Scenario scenario)
        {
            ScenarioList.Add(scenario.Name, scenario);
            return scenario;
        }

        public static void Change(string name)
        {
            LoadScenario(ScenarioList[name]);
        }

        public static Scenario Get(string name)
        {
            return ScenarioList[name];
        }
    }
}