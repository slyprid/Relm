﻿using System.Collections.Generic;
using ImGuiNET;
using Microsoft.Xna.Framework.Graphics;
using Relm.Gui.Utils;

namespace Relm.Gui.Inspectors.TypeInspectors
{
    public class EffectInspector : AbstractTypeInspector
    {
        public bool AllowsEffectRemoval = true;

        List<AbstractTypeInspector> _inspectors = new List<AbstractTypeInspector>();

        public override void Initialize()
        {
            base.Initialize();

            var effect = GetValue<Effect>();
            _name += $" ({effect.GetType().Name})";

            var inspectors = TypeInspectorUtils.GetInspectableProperties(effect);
            foreach (var inspector in inspectors)
            {
                // we dont need the Name field. It serves no purpose.
                if (inspector.Name != "Name")
                    _inspectors.Add(inspector);
            }
        }

        public override void DrawMutable()
        {
            var isOpen = ImGui.CollapsingHeader($"{_name}", ImGuiTreeNodeFlags.FramePadding);

            if (AllowsEffectRemoval)
                RelmImGui.ShowContextMenuTooltip();

            if (AllowsEffectRemoval && ImGui.BeginPopupContextItem())
            {
                if (ImGui.Selectable("Remove Effect"))
                {
                    SetValue(null);
                    _isTargetDestroyed = true;
                }

                ImGui.EndPopup();
            }

            if (isOpen && !_isTargetDestroyed)
            {
                foreach (var i in _inspectors)
                    i.Draw();
            }
        }
    }
}