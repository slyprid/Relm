using System.Collections.Generic;

namespace Relm.Collections
{
    public static class ListPool<T>
    {
        static readonly Queue<List<T>> _objectQueue = new Queue<List<T>>();


        public static void WarmCache(int cacheCount)
        {
            cacheCount -= _objectQueue.Count;
            if (cacheCount > 0)
            {
                for (var i = 0; i < cacheCount; i++)
                    _objectQueue.Enqueue(new List<T>());
            }
        }

        public static void TrimCache(int cacheCount)
        {
            while (cacheCount > _objectQueue.Count)
                _objectQueue.Dequeue();
        }

        public static void ClearCache()
        {
            _objectQueue.Clear();
        }

        public static List<T> Obtain()
        {
            if (_objectQueue.Count > 0)
                return _objectQueue.Dequeue();

            return new List<T>();
        }

        public static void Free(List<T> obj)
        {
            _objectQueue.Enqueue(obj);
            obj.Clear();
        }
    }
}