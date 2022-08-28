using System.Collections.Generic;
using System;

namespace Relm.Collections
{
    public struct Pair<T> 
        : IEquatable<Pair<T>> where T : class
    {
        public T First;
        public T Second;

        public Pair(T first, T second)
        {
            First = first;
            Second = second;
        }

        public void Clear()
        {
            First = Second = null;
        }

        public bool Equals(Pair<T> other)
        {
            return First == other.First && Second == other.Second;
        }
        
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(First) * 37 + EqualityComparer<T>.Default.GetHashCode(Second);
        }
    }
}