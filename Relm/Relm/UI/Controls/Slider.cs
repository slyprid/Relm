using Relm.States;

namespace Relm.UI.Controls
{
    public class Slider
        : Control
    {
        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(Slider)];

        public override void InitializeEvents()
        {
            throw new System.NotImplementedException();
        }
    }
}