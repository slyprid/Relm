using Microsoft.Xna.Framework;
using Relm.UI.Drawable;
using IDrawable = Relm.UI.Drawable.IDrawable;

namespace Relm.UI.Widgets.Styles
{
    public class SliderStyle : ProgressBarStyle
    {
        /** Optional. */
        public IDrawable KnobOver, KnobDown;


        public SliderStyle()
        {
        }


        public SliderStyle(IDrawable background, IDrawable knob) : base(background, knob)
        {
        }


        public new static SliderStyle Create(Color backgroundColor, Color knobColor)
        {
            var background = new PrimitiveDrawable(backgroundColor);
            background.MinWidth = background.MinHeight = 10;

            var knob = new PrimitiveDrawable(knobColor);
            knob.MinWidth = knob.MinHeight = 20;

            return new SliderStyle
            {
                Background = background,
                Knob = knob
            };
        }


        public new SliderStyle Clone()
        {
            return new SliderStyle
            {
                Background = Background,
                DisabledBackground = DisabledBackground,
                Knob = Knob,
                DisabledKnob = DisabledKnob,
                KnobBefore = KnobBefore,
                KnobAfter = KnobAfter,
                DisabledKnobBefore = DisabledKnobBefore,
                DisabledKnobAfter = DisabledKnobAfter,

                KnobOver = KnobOver,
                KnobDown = KnobDown
            };
        }
    }
}