using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using Relm.Core;
using Relm.Graphics.PostProcessing;
using Relm.Gui.Inspectors.ObjectInspectors;
using Relm.Gui.Utils;

namespace Relm.Gui.Inspectors.SceneGraphPanes
{
	public class PostProcessorsPane
	{
		List<PostProcessorInspector> _postProcessorInspectors = new List<PostProcessorInspector>();
		bool _isPostProcessorListInitialized;

		void UpdatePostProcessorInspectorList()
		{
			// first, we check our list of inspectors and sync it up with the current list of PostProcessors in the Scene.
			// we limit the check to once every 60 fames
			if (!_isPostProcessorListInitialized || Time.FrameCount % 60 == 0)
			{
				_isPostProcessorListInitialized = true;
				for (var i = 0; i < RelmGame.Scene._postProcessors.Length; i++)
				{
					var postProcessor = RelmGame.Scene._postProcessors.Buffer[i];
					if (_postProcessorInspectors.Where(inspector => inspector.PostProcessor == postProcessor).Count() ==
						0)
						_postProcessorInspectors.Add(new PostProcessorInspector(postProcessor));
				}
			}
		}

		public void OnSceneChanged()
		{
			_postProcessorInspectors.Clear();
			_isPostProcessorListInitialized = false;
			UpdatePostProcessorInspectorList();
		}

		public void Draw()
		{
			UpdatePostProcessorInspectorList();

			ImGui.Indent();
			for (var i = 0; i < _postProcessorInspectors.Count; i++)
			{
				if (_postProcessorInspectors[i].PostProcessor._scene != null)
				{
					_postProcessorInspectors[i].Draw();
					RelmImGui.SmallVerticalSpace();
				}
			}

			if (_postProcessorInspectors.Count == 0)
				RelmImGui.SmallVerticalSpace();

			if (RelmImGui.CenteredButton("Add PostProcessor", 0.6f))
			{
				ImGui.OpenPopup("postprocessor-selector");
			}

			ImGui.Unindent();

			RelmImGui.MediumVerticalSpace();
			DrawPostProcessorSelectorPopup();
		}

		void DrawPostProcessorSelectorPopup()
		{
			if (ImGui.BeginPopup("postprocessor-selector"))
			{
				foreach (var subclassType in InspectorCache.GetAllPostProcessorSubclassTypes())
				{
					if (ImGui.Selectable(subclassType.Name))
					{
						var postprocessor = (PostProcessor)Activator.CreateInstance(subclassType,
							new object[] { _postProcessorInspectors.Count });
                        RelmGame.Scene.AddPostProcessor(postprocessor);
						_isPostProcessorListInitialized = false;
					}
				}

				ImGui.EndPopup();
			}
		}
	}
}