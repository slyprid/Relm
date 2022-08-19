using System.Collections.Generic;

namespace Relm.Input
{
	public struct InputEventTypeComparer : IEqualityComparer<InputEventType>
    {
        public bool Equals(InputEventType x, InputEventType y)
        {
            return x == y;
        }


        public int GetHashCode(InputEventType obj)
        {
            return (int)obj;
        }
    }
}