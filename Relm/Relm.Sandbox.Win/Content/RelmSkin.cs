using Microsoft.Xna.Framework.Input;
using Relm.UI;
using Relm.UI.Configuration;
using Relm.UI.Controls;

namespace Relm.Sandbox.Win.Content
{
    public class RelmSkin
        : UserInterfaceSkin
    {
        public RelmSkin()
        {
            Add<Button, ButtonConfig>()
                .With(UI.States.ButtonState.Normal, 0, 0, 128, 64)
                .With(UI.States.ButtonState.Hover, 0, 64, 128, 64)
                .With(UI.States.ButtonState.Active, 0, 128, 128, 64)
                .SizeOf(128, 64);
        }
    }
}