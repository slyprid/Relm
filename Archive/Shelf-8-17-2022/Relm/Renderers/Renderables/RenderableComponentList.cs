using System.Collections.Generic;
using Relm.Collections;

namespace Relm.Renderers.Renderables
{
    public class RenderableComponentList
        : SafeList<IRenderable>
    {
        private Dictionary<int, List<IRenderable>> _itemsByRenderLayer = new Dictionary<int, List<IRenderable>>();

        public override void Add(IRenderable item)
        {
            base.Add(item);
            AddToRenderLayerList(item, item.RenderLayer);
        }

        public override void Remove(IRenderable item)
        {
            base.Remove(item);
            _itemsByRenderLayer[item.RenderLayer].Remove(item);
        }

        public void UpdateRenderableRenderLayer(IRenderable component, int oldRenderLayer, int newRenderLayer)
        {
            if (_itemsByRenderLayer.ContainsKey(oldRenderLayer) &&
                _itemsByRenderLayer[oldRenderLayer].Contains(component))
            {
                _itemsByRenderLayer[oldRenderLayer].Remove(component);
                AddToRenderLayerList(component, component.RenderLayer);
            }
        }

        public void AddToRenderLayerList(IRenderable component, int renderLayer)
        {
            var list = ComponentsWithRenderLayer(renderLayer);
            list.Add(component);
        }

        public List<IRenderable> ComponentsWithRenderLayer(int renderLayer)
        {
            if (!_itemsByRenderLayer.TryGetValue(renderLayer, out _))
            {
                _itemsByRenderLayer[renderLayer] = new List<IRenderable>();
            }
            return _itemsByRenderLayer[renderLayer];
        }
    }
}