using Relm.Components;
using Relm.Entities;

namespace Relm.Gui.Inspectors.ObjectInspectors
{
    public interface IComponentInspector
    {
        Entity Entity { get; }
        Component Component { get; }

        void Draw();
    }
}