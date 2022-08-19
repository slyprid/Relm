using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
	public class WindowStyle
    {
        public BitmapFont TitleFont;

        /** Optional. */
        public float TitleFontScaleX = 1f;

        /** Optional. */
        public float TitleFontScaleY = 1f;

        /** Optional. */
        public IDrawable Background;

        /** Optional. */
        public Color TitleFontColor = Color.White;

        /** Optional. */
        public IDrawable StageBackground;




        public WindowStyle()
        {
            TitleFont = RelmGraphics.Instance.BitmapFont;
        }


        public WindowStyle(BitmapFont titleFont, Color titleFontColor, IDrawable background) : this(titleFont, titleFontColor, background, 1f)
        { }


        public WindowStyle(BitmapFont titleFont, Color titleFontColor, IDrawable background, float titleFontScale) : this(titleFont, titleFontColor, background, titleFontScale, titleFontScale)
        { }


        public WindowStyle(BitmapFont titleFont, Color titleFontColor, IDrawable background, float titleFontScaleX, float titleFontScaleY)
        {
            TitleFont = titleFont ?? RelmGraphics.Instance.BitmapFont;
            Background = background;
            TitleFontColor = titleFontColor;
            TitleFontScaleX = titleFontScaleX;
            TitleFontScaleY = titleFontScaleY;
        }


        public WindowStyle Clone()
        {
            return new WindowStyle
            {
                Background = Background,
                TitleFont = TitleFont,
                TitleFontColor = TitleFontColor,
                TitleFontScaleX = TitleFontScaleX,
                TitleFontScaleY = TitleFontScaleY,
                StageBackground = StageBackground
            };
        }
    }
}