using Microsoft.Xna.Framework;
using Relm.Assets.BitmapFonts;
using Relm.UI.Drawable;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
    public class TextButtonStyle : ButtonStyle
    {
        public BitmapFont Font;

        /** Optional. */
        public Color FontColor = Color.White;
        public Color? DownFontColor, OverFontColor, CheckedFontColor, CheckedOverFontColor, DisabledFontColor;
        public float FontScaleX = 1;
        public float FontScaleY = 1;
        public float FontScale { set { FontScaleX = value; FontScaleY = value; } }


        public TextButtonStyle()
        {
            Font = RelmGraphics.Instance.BitmapFont;
        }


        public TextButtonStyle(IDrawable up, IDrawable down, IDrawable over, BitmapFont font) : base(up, down, over)
        {
            Font = font ?? RelmGraphics.Instance.BitmapFont;
        }


        public TextButtonStyle(IDrawable up, IDrawable down, IDrawable over) : this(up, down, over,
            RelmGraphics.Instance.BitmapFont)
        {
        }


        public new static TextButtonStyle Create(Color upColor, Color downColor, Color overColor)
        {
            return new TextButtonStyle
            {
                Up = new PrimitiveDrawable(upColor),
                Down = new PrimitiveDrawable(downColor),
                Over = new PrimitiveDrawable(overColor)
            };
        }


        public new TextButtonStyle Clone()
        {
            return new TextButtonStyle
            {
                Up = Up,
                Down = Down,
                Over = Over,
                Checked = Checked,
                CheckedOver = CheckedOver,
                Disabled = Disabled,

                Font = Font,
                FontColor = FontColor,
                DownFontColor = DownFontColor,
                OverFontColor = OverFontColor,
                CheckedFontColor = CheckedFontColor,
                CheckedOverFontColor = CheckedOverFontColor,
                DisabledFontColor = DisabledFontColor,
                FontScaleX = FontScaleX,
                FontScaleY = FontScaleY,
            };
        }
    }
}