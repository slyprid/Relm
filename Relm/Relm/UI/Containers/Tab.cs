using Relm.UI.Base;
using Relm.UI.Containers.Styles;

namespace Relm.UI.Containers
{
    public class Tab : Table
    {
        private TabStyle _style;
        public string TabName;

        public Tab(string name, TabStyle style)
        {
            TabName = name;
            _style = style;
            SetTouchable(Touchable.Enabled);
            Setup();
        }

        private void Setup()
        {
            SetBackground(_style.Background);
            SetFillParent(true);
            Top().Left();
        }
    }
}