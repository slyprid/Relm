using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
    public class ListBoxStyle
    {
        public BitmapFont Font;
        public Color FontColorSelected = Color.Black;
        public Color FontColorUnselected = Color.White;
        public Color FontColorHovered = Color.Black;

        public IDrawable Selection;

        /** Optional */
        public IDrawable HoverSelection;

        /** Optional */
        public IDrawable Background;


        public ListBoxStyle()
        {
            Font = RelmGraphics.Instance.BitmapFont;
        }


        public ListBoxStyle(BitmapFont font, Color fontColorSelected, Color fontColorUnselected, IDrawable selection)
        {
            Font = font;
            FontColorSelected = fontColorSelected;
            FontColorUnselected = fontColorUnselected;
            Selection = selection;
        }


        public ListBoxStyle Clone()
        {
            return new ListBoxStyle
            {
                Font = Font,
                FontColorSelected = FontColorSelected,
                FontColorUnselected = FontColorUnselected,
                Selection = Selection,
                HoverSelection = HoverSelection,
                Background = Background
            };
        }
    }
}