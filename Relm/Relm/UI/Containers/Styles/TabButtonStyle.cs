using Relm.UI.Drawable;
using Relm.UI.Widgets.Styles;

namespace Relm.UI.Containers.Styles
{
    public class TabButtonStyle
    {
        public IDrawable Active;
        public IDrawable Inactive;
        public IDrawable Locked;
        public IDrawable Hover;
        public float PaddingTop = 0.0F;
        public LabelStyle LabelStyle;
    }
}