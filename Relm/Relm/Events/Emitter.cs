using System;
using System.Collections.Generic;

namespace Relm.Events
{
	public class Emitter<T> 
        where T : struct, IComparable, IFormattable
	{
		Dictionary<T, List<Action>> _messageTable;

        public Emitter()
		{
			_messageTable = new Dictionary<T, List<Action>>();
		}

		public Emitter(IEqualityComparer<T> customComparer)
		{
			_messageTable = new Dictionary<T, List<Action>>(customComparer);
		}

		public void AddObserver(T eventType, Action handler)
		{
			List<Action> list = null;
			if (!_messageTable.TryGetValue(eventType, out list))
			{
				list = new List<Action>();
				_messageTable.Add(eventType, list);
			}

			Assert.IsFalse(list.Contains(handler), "You are trying to add the same observer twice");
			list.Add(handler);
		}

		public void RemoveObserver(T eventType, Action handler)
		{
			_messageTable[eventType].Remove(handler);
		}

		public void Emit(T eventType)
		{
			List<Action> list = null;
			if (_messageTable.TryGetValue(eventType, out list))
			{
				for (var i = list.Count - 1; i >= 0; i--)
					list[i]();
			}
		}
	}


	public class Emitter<T, U> 
        where T : struct, IComparable, IFormattable
	{
		Dictionary<T, List<Action<U>>> _messageTable;
		
		public Emitter()
		{
			_messageTable = new Dictionary<T, List<Action<U>>>();
		}

		public Emitter(IEqualityComparer<T> customComparer)
		{
			_messageTable = new Dictionary<T, List<Action<U>>>(customComparer);
		}

		public void AddObserver(T eventType, Action<U> handler)
		{
			List<Action<U>> list = null;
			if (!_messageTable.TryGetValue(eventType, out list))
			{
				list = new List<Action<U>>();
				_messageTable.Add(eventType, list);
			}

			Assert.IsFalse(list.Contains(handler), "You are trying to add the same observer twice");
			list.Add(handler);
		}

		public void RemoveObserver(T eventType, Action<U> handler)
		{
			_messageTable[eventType].Remove(handler);
		}

		public void Emit(T eventType, U data)
		{
			List<Action<U>> list = null;
			if (_messageTable.TryGetValue(eventType, out list))
			{
				for (var i = list.Count - 1; i >= 0; i--)
					list[i](data);
			}
		}
	}
}