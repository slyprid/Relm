using System.Collections.Generic;

namespace Relm.Events
{
	public struct CoreEventsComparer : IEqualityComparer<CoreEvents>
    {
        public bool Equals(CoreEvents x, CoreEvents y)
        {
            return x == y;
        }


        public int GetHashCode(CoreEvents obj)
        {
            return (int)obj;
        }
    }
}