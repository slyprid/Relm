using System.Collections.Generic;
using Relm.Components;
using Relm.Entities;
using Relm.Gui.Inspectors.TypeInspectors;
using Relm.Gui.Utils;

namespace Relm.Gui.Inspectors.ObjectInspectors
{
    public abstract class AbstractComponentInspector : IComponentInspector
    {
        public abstract Entity Entity { get; }
        public abstract Component Component { get; }

        protected List<AbstractTypeInspector> _inspectors;
        protected int _scopeId = RelmImGui.GetScopeId();

        public abstract void Draw();
    }
}