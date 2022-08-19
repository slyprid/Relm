using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
    public class CheckBoxStyle : TextButtonStyle
    {
        public IDrawable CheckboxOn, CheckboxOff;

        /** Optional. */
        public IDrawable CheckboxOver, CheckboxOnDisabled, CheckboxOffDisabled;


        public CheckBoxStyle()
        {
            Font = RelmGraphics.Instance.BitmapFont;
        }


        public CheckBoxStyle(IDrawable checkboxOff, IDrawable checkboxOn, BitmapFont font, Color fontColor)
        {
            CheckboxOff = checkboxOff;
            CheckboxOn = checkboxOn;
            Font = font ?? RelmGraphics.Instance.BitmapFont;
            FontColor = fontColor;
        }
    }
}