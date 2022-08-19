using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using Relm.UI.Containers.Styles;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
    public class SelectBoxStyle
    {
        public BitmapFont Font;

        public Color FontColor = Color.White;

        /** Optional */
        public Color DisabledFontColor;

        /** Optional */
        public IDrawable Background;
        public ScrollPaneStyle ScrollStyle;

        public ListBoxStyle ListStyle;

        /** Optional */
        public IDrawable BackgroundOver, BackgroundOpen, BackgroundDisabled;


        public SelectBoxStyle()
        {
            Font = RelmGraphics.Instance.BitmapFont;
        }

        public SelectBoxStyle(BitmapFont font, Color fontColor, IDrawable background, ScrollPaneStyle scrollStyle,
            ListBoxStyle listStyle)
        {
            Font = font;
            FontColor = fontColor;
            Background = background;
            ScrollStyle = scrollStyle;
            ListStyle = listStyle;
        }
    }
}