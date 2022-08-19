using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
	public class LabelStyle
    {
        public Color FontColor = Color.White;
        public BitmapFont Font;
        public IDrawable Background;
        public float FontScaleX = 1f;
        public float FontScaleY = 1f;
        public float FontScale { set { FontScaleX = value; FontScaleY = value; } }


        public LabelStyle()
        {
            Font = RelmGraphics.Instance.BitmapFont;
        }


        public LabelStyle(BitmapFont font, Color fontColor) : this(font, fontColor, 1f)
        { }


        public LabelStyle(BitmapFont font, Color fontColor, float fontScaleX, float fontScaleY)
        {
            Font = font ?? RelmGraphics.Instance.BitmapFont;
            FontColor = fontColor;
            FontScaleX = fontScaleX;
            FontScaleY = fontScaleY;
        }


        public LabelStyle(BitmapFont font, Color fontColor, float fontScale) : this(font, fontColor, fontScale, fontScale)
        {

        }


        public LabelStyle(Color fontColor) : this(null, fontColor)
        { }


        public LabelStyle Clone()
        {
            return new LabelStyle
            {
                FontColor = FontColor,
                Font = Font,
                Background = Background,
                FontScaleX = FontScaleX,
                FontScaleY = FontScaleY
            };
        }
    }
}