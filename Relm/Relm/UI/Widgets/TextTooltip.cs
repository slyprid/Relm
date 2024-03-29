﻿using Relm.UI.Base;
using Relm.UI.Widgets.Styles;

namespace Relm.UI.Widgets
{
    public class TextTooltip : Tooltip
    {
        public TextTooltip(string text, Element targetElement, Skin skin, string styleName = null) : this(text,
            targetElement, skin.Get<TextTooltipStyle>(styleName))
        {
        }


        public TextTooltip(string text, Element targetElement, TextTooltipStyle style) : base(null, targetElement)
        {
            var label = new Label(text, style.LabelStyle);
            _container.SetElement(label);
            SetStyle(style);
        }


        public TextTooltip SetStyle(TextTooltipStyle style)
        {
            _container.GetElement<Label>().SetStyle(style.LabelStyle);
            _container.SetBackground(style.Background);
            return this;
        }
    }
}