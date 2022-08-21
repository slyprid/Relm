using Relm.Entities;
using Relm.Gui.Utils;

namespace Relm.Gui.Inspectors.TypeInspectors
{
    public class EntityFieldInspector : AbstractTypeInspector
    {
        public override void DrawMutable()
        {
            var entity = GetValue<Entity>();

            if (RelmImGui.LabelButton(_name, entity.Name))
                RelmGame.GetGlobalManager<ImGuiManager>().StartInspectingEntity(entity);
            HandleTooltip();
        }
    }
}