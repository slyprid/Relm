using System.Collections.Generic;

namespace Relm.Core.Managers
{
    public abstract class Manager<T>
        : IManager<T>
        where T : IManaged<T>
    {
        private readonly List<string> _itemNames;
        private readonly List<T> _items;

        public List<T> Items => _items;

        public T this[string alias] => GetItem(alias);

        protected Manager()
        {
            _itemNames = new List<string>();
            _items = new List<T>();
        }

        public virtual void Add(string alias, T item)
        {
            item.Manager = this;
            item.MyAlias = alias;
            _itemNames.Add(alias);
            _items.Add(item);
        }

        public virtual void Remove(string alias)
        {
            var index = _itemNames.IndexOf(alias);
            _itemNames.RemoveAt(index);
            _items.RemoveAt(index);
        }

        public virtual void Remove(T item)
        {
            var index = _items.IndexOf(item);
            _itemNames.RemoveAt(index);
            _items.RemoveAt(index);
        }

        public virtual void Clear()
        {
            _itemNames.Clear();
            _items.Clear();
        }

        public T GetItem(string alias)
        {
            var index = _itemNames.IndexOf(alias);
            return _items[index];
        }
    }
}