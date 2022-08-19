using Relm.UI.Drawable;

namespace Relm.UI.Widgets.Styles
{
    public class TextTooltipStyle
    {
        public LabelStyle LabelStyle;

        /** Optional. */
        public IDrawable Background;


        public TextTooltipStyle()
        {
        }


        public TextTooltipStyle(LabelStyle label, IDrawable background)
        {
            LabelStyle = label;
            Background = background;
        }
    }
}