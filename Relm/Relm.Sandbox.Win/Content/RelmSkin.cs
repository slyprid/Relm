using Relm.UI;
using Relm.UI.Configuration;
using Relm.UI.Controls;
using Relm.UI.States;
using Button = Relm.UI.Controls.Button;

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

            Add<Panel, PanelConfig>()
                .With(PanelPiece.TopLeft, 128, 0, 32, 32)
                .With(PanelPiece.Top, 160, 0, 32, 32)
                .With(PanelPiece.TopRight, 192, 0, 32, 32)
                .With(PanelPiece.Left, 128, 32, 32, 32)
                .With(PanelPiece.Center, 160, 32, 32, 32)
                .With(PanelPiece.Right, 192, 32, 32, 32)
                .With(PanelPiece.BottomLeft, 128, 64, 32, 32)
                .With(PanelPiece.Bottom, 160, 64, 32, 32)
                .With(PanelPiece.BottomRight, 192, 64, 32, 32)
                .SizeOf(96, 96);

        }
    }
}