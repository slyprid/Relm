using System;
using System.Collections;
using System.Collections.Generic;

namespace Relm.Collections
{
	public class FastList<T>
	{
		public T[] Buffer;
        public int Length = 0;
        public T this[int index] => Buffer[index];

		public FastList(int size)
		{
			Buffer = new T[size];
		}
		
		public FastList() : this(5) { }
		
		public void Clear()
		{
			Array.Clear(Buffer, 0, Length);
			Length = 0;
		}
		
		public void Reset()
		{
			Length = 0;
		}

		public void Add(T item)
		{
			if (Length == Buffer.Length) Array.Resize(ref Buffer, System.Math.Max(Buffer.Length << 1, 10));
			Buffer[Length++] = item;
		}
		
		public void Remove(T item)
		{
			var comp = EqualityComparer<T>.Default;
			for (var i = 0; i < Length; ++i)
			{
				if (comp.Equals(Buffer[i], item))
				{
					RemoveAt(i);
					return;
				}
			}
		}
		
		public void RemoveAt(int index)
		{
			Assert.IsTrue(index < Length, "Index out of range!");

			Length--;
			if (index < Length)
				Array.Copy(Buffer, index + 1, Buffer, index, Length - index);
			Buffer[Length] = default(T);
		}
		
		public void RemoveAtWithSwap(int index)
		{
			Assert.IsTrue(index < Length, "Index out of range!");

			Buffer[index] = Buffer[Length - 1];
			Buffer[Length - 1] = default(T);
			--Length;
		}
		
		public bool Contains(T item)
		{
			var comp = EqualityComparer<T>.Default;
			for (var i = 0; i < Length; ++i)
			{
				if (comp.Equals(Buffer[i], item))
					return true;
			}

			return false;
		}
		
		public void EnsureCapacity(int additionalItemCount = 1)
		{
			if (Length + additionalItemCount >= Buffer.Length) Array.Resize(ref Buffer, System.Math.Max(Buffer.Length << 1, Length + additionalItemCount));
		}
		
		public void AddRange(IEnumerable<T> array)
		{
			foreach (var item in array) Add(item);
		}
		
		public void Sort()
		{
			Array.Sort(Buffer, 0, Length);
		}
		
		public void Sort(IComparer comparer)
		{
			Array.Sort(Buffer, 0, Length, comparer);
		}
		
		public void Sort(IComparer<T> comparer)
		{
			Array.Sort(Buffer, 0, Length, comparer);
		}
	}
}