using System.Collections.Generic;

namespace Relm.Components
{
    public class IUpdateableComparer 
        : IComparer<IUpdateable>
    {
        public int Compare(IUpdateable a, IUpdateable b)
        {
            return a.UpdateOrder.CompareTo(b.UpdateOrder);
        }
    }
}