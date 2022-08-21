using System.Collections.Generic;
using ImGuiNET;
using Relm.Gui.Utils;

namespace Relm.Gui.Inspectors.TypeInspectors
{
	public class StructInspector : AbstractTypeInspector
	{
		List<AbstractTypeInspector> _inspectors = new List<AbstractTypeInspector>();
		bool _isHeaderOpen;

		public override void Initialize()
		{
			base.Initialize();

			// figure out which fields and properties are useful to add to the inspector
			var fields = Relm.Utils.GetFields(_valueType);
			foreach (var field in fields)
			{
				if (!field.IsPublic && !field.IsDefined(typeof(InspectableAttribute), false))
					continue;

				var inspector = TypeInspectorUtils.GetInspectorForType(field.FieldType, _target, field);
				if (inspector != null)
				{
					inspector.SetStructTarget(_target, this, field);
					inspector.Initialize();
					_inspectors.Add(inspector);
				}
			}

			var properties = Relm.Utils.GetProperties(_valueType);
			foreach (var prop in properties)
			{
				if (!prop.CanRead || !prop.CanWrite)
					continue;

				var isPropertyUndefinedOrPublic = !prop.CanWrite || (prop.CanWrite && prop.SetMethod.IsPublic);
				if ((!prop.GetMethod.IsPublic || !isPropertyUndefinedOrPublic) &&
					!prop.IsDefined(typeof(InspectableAttribute), false))
					continue;

				var inspector = TypeInspectorUtils.GetInspectorForType(prop.PropertyType, _target, prop);
				if (inspector != null)
				{
					inspector.SetStructTarget(_target, this, prop);
					inspector.Initialize();
					_inspectors.Add(inspector);
				}
			}
		}

		public override void DrawMutable()
		{
			ImGui.Indent();
            RelmImGui.BeginBorderedGroup();

			_isHeaderOpen = ImGui.CollapsingHeader($"{_name}");
			if (_isHeaderOpen)
			{
				foreach (var i in _inspectors)
					i.Draw();
			}

            RelmImGui.EndBorderedGroup(new System.Numerics.Vector2(4, 1), new System.Numerics.Vector2(4, 2));
			ImGui.Unindent();
		}

		/// <summary>
		/// we need to override here so that we can keep the header enabled so that it can be opened
		/// </summary>
		public override void DrawReadOnly()
		{
			DrawMutable();
		}
	}
}