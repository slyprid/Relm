using System;

namespace Relm.UI.Widgets.Filters
{
    public class DigitsOnlyFilter : TextField.ITextFieldFilter
    {
        public bool AcceptChar(TextField textField, char c)
        {
            return Char.IsDigit(c) || c == '-';
        }
    }
}