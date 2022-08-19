using System.Collections.Generic;
using Relm.Collections;

namespace Relm.Components
{
	public class RenderableComponentList
	{
		public static IComparer<IRenderable> CompareUpdatableOrder = new RenderableComparer();
        private FastList<IRenderable> _components = new FastList<IRenderable>();
        private Dictionary<int, FastList<IRenderable>> _componentsByRenderLayer = new Dictionary<int, FastList<IRenderable>>();
        private List<int> _unsortedRenderLayers = new List<int>();
		private bool _componentsNeedSort = true;

		#region array access

		public int Count => _components.Length;

		public IRenderable this[int index] => _components.Buffer[index];

		#endregion


		public void Add(IRenderable component)
		{
			_components.Add(component);
			AddToRenderLayerList(component, component.RenderLayer);
		}

		public void Remove(IRenderable component)
		{
			_components.Remove(component);
			_componentsByRenderLayer[component.RenderLayer].Remove(component);
		}

		public void UpdateRenderableRenderLayer(IRenderable component, int oldRenderLayer, int newRenderLayer)
		{
			if (_componentsByRenderLayer.ContainsKey(oldRenderLayer) && _componentsByRenderLayer[oldRenderLayer].Contains(component))
			{
				_componentsByRenderLayer[oldRenderLayer].Remove(component);
				AddToRenderLayerList(component, newRenderLayer);
			}
		}

		public void SetRenderLayerNeedsComponentSort(int renderLayer)
		{
			if (!_unsortedRenderLayers.Contains(renderLayer))
				_unsortedRenderLayers.Add(renderLayer);
			_componentsNeedSort = true;
		}

		internal void SetNeedsComponentSort() => _componentsNeedSort = true;

		void AddToRenderLayerList(IRenderable component, int renderLayer)
		{
			var list = ComponentsWithRenderLayer(renderLayer);
			Assert.IsFalse(list.Contains(component), "Component renderLayer list already contains this component");

			list.Add(component);
			if (!_unsortedRenderLayers.Contains(renderLayer))
				_unsortedRenderLayers.Add(renderLayer);
			_componentsNeedSort = true;
		}

		public FastList<IRenderable> ComponentsWithRenderLayer(int renderLayer)
		{
			if (!_componentsByRenderLayer.TryGetValue(renderLayer, out _))
				_componentsByRenderLayer[renderLayer] = new FastList<IRenderable>();

			return _componentsByRenderLayer[renderLayer];
		}

		public void UpdateLists()
		{
			if (_componentsNeedSort)
			{
				_components.Sort(CompareUpdatableOrder);
				_componentsNeedSort = false;
			}

			if (_unsortedRenderLayers.Count > 0)
			{
				for (int i = 0, count = _unsortedRenderLayers.Count; i < count; i++)
				{
					if (_componentsByRenderLayer.TryGetValue(_unsortedRenderLayers[i], out var renderLayerComponents))
						renderLayerComponents.Sort(CompareUpdatableOrder);
				}

				_unsortedRenderLayers.Clear();
			}
		}
	}
}