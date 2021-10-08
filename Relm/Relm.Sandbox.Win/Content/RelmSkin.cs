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

            Add<ProgressBar, ProgressBarConfig>()
                .With(ProgressBarPiece.LeftOverlay, 128, 96, 32, 32)
                .With(ProgressBarPiece.CenterOverlay, 160, 96, 32, 32)
                .With(ProgressBarPiece.RightOverlay, 192, 96, 32, 32)
                .With(ProgressBarPiece.LeftFill, 128, 128, 32, 32)
                .With(ProgressBarPiece.CenterFill, 160, 128, 32, 32)
                .With(ProgressBarPiece.RightFill, 192, 128, 32, 32)
                .With(ProgressBarPiece.LeftFillRounded, 128, 128, 8, 32)
                .With(ProgressBarPiece.RightFillRounded, 216, 128, 8, 32)
                .SizeOf(96, 32);

            Add<CheckBox, CheckBoxConfig>()
                .With(CheckBoxState.Unchecked, 128, 160, 32, 32)
                .With(CheckBoxState.Checked, 160, 160, 32, 32)
                .SizeOf(32, 32);

            Add<TextBox, TextBoxConfig>()
                .With(TextBoxPiece.Left, 224, 0, 8, 32)
                .With(TextBoxPiece.Center, 232, 0, 80, 32)
                .With(TextBoxPiece.Right, 312, 0, 8, 32)
                .SizeOf(96, 32);

            Add<ScrollBar, ScrollBarConfig>()
                .With(ScrollBarPiece.LeftButtonNormal, 224, 32, 32, 32)
                .With(ScrollBarPiece.LeftButtonHover, 256, 32, 32, 32)
                .With(ScrollBarPiece.LeftButtonActive, 288, 32, 32, 32)
                .With(ScrollBarPiece.HorizontalBar, 256, 160, 32, 32)
                .With(ScrollBarPiece.HorizontalSlider, 224, 192, 32, 32)
                .With(ScrollBarPiece.HorizontalSliderHover, 288, 192, 32, 32)
                .With(ScrollBarPiece.VerticalSlider, 224, 160, 32, 32)
                .With(ScrollBarPiece.VerticalSliderHover, 288, 160, 32, 32)
                .With(ScrollBarPiece.RightButtonNormal, 224, 64, 32, 32)
                .With(ScrollBarPiece.RightButtonHover, 256, 64, 32, 32)
                .With(ScrollBarPiece.RightButtonActive, 288, 64, 32, 32)
                .With(ScrollBarPiece.UpButtonNormal, 224, 96, 32, 32)
                .With(ScrollBarPiece.UpButtonHover, 256, 96, 32, 32)
                .With(ScrollBarPiece.UpButtonActive, 288, 96, 32, 32)
                .With(ScrollBarPiece.VerticalBar, 256, 192, 32, 32)
                .With(ScrollBarPiece.DownButtonNormal, 224, 128, 32, 32)
                .With(ScrollBarPiece.DownButtonHover, 256, 128, 32, 32)
                .With(ScrollBarPiece.DownButtonActive, 288, 128, 32, 32)
                .SizeOf(96, 32);

            Add<ListBox, ListBoxConfig>()
                .With(PanelPiece.TopLeft, 320, 0, 32, 32)
                .With(PanelPiece.Top, 352, 0, 32, 32)
                .With(PanelPiece.TopRight, 384, 0, 32, 32)
                .With(PanelPiece.Left, 320, 32, 32, 32)
                .With(PanelPiece.Center, 352, 32, 32, 32)
                .With(PanelPiece.Right, 384, 32, 32, 32)
                .With(PanelPiece.BottomLeft, 320, 64, 32, 32)
                .With(PanelPiece.Bottom, 352, 64, 32, 32)
                .With(PanelPiece.BottomRight, 384, 64, 32, 32);

        }
    }
}