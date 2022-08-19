using System.Collections.Generic;

namespace Relm.Components
{
    public class RenderableComparer 
        : IComparer<IRenderable>
    {
        public int Compare(IRenderable self, IRenderable other)
        {
            var res = other.RenderLayer.CompareTo(self.RenderLayer);
            if (res == 0)
            {
                res = other.LayerDepth.CompareTo(self.LayerDepth);
                if (res == 0)
                {
                    if (ReferenceEquals(self.Material, other.Material)) return 0;
                    if (other.Material == null) return -1;
                    return 1;
                }
            }

            return res;
        }
    }
}