using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.Collections
{
    public class SafeList<T>
        where T : class
    {
        protected readonly List<T> Items = new List<T>();
        protected readonly List<T> ItemsToUpdate = new List<T>();
        protected readonly List<T> ItemsToAdd = new List<T>();
        protected readonly List<T> ItemsToRemove = new List<T>();
        protected readonly List<T> TempItems = new List<T>();

        public T this[int index] => Items[index];

        public virtual void Add(T item)
        {
            ItemsToAdd.Add(item);
        }

        public virtual void Remove(T item)
        {
            if (ItemsToAdd.Contains(item))
            {
                ItemsToAdd.Remove(item);
                return;
            }

            ItemsToRemove.Add(item);
        }

        public virtual void RemoveAll()
        {
            foreach (var item in Items) HandleRemove(item);

            Items.Clear();
            ItemsToUpdate.Clear();
            ItemsToAdd.Clear();
            ItemsToRemove.Clear();
        }

        public virtual void UpdateLists() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void HandleRemove(T item) { }
    }
}