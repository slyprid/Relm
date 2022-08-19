using System.Collections.Generic;

namespace Relm.Collections
{
	public static class Pool<T> where T : new()
    {
        private static readonly Queue<T> ObjectQueue = new Queue<T>(10);
        
        public static void WarmCache(int cacheCount)
        {
            cacheCount -= ObjectQueue.Count;
            if (cacheCount <= 0) return;
            for (var i = 0; i < cacheCount; i++) ObjectQueue.Enqueue(new T());
        }
        
        public static void TrimCache(int cacheCount)
        {
            while (cacheCount > ObjectQueue.Count) ObjectQueue.Dequeue();
        }
        
        public static void ClearCache()
        {
            ObjectQueue.Clear();
        }
        
        public static T Obtain()
        {
            return ObjectQueue.Count > 0 ? ObjectQueue.Dequeue() : new T();
        }
        
        public static void Free(T obj)
        {
            ObjectQueue.Enqueue(obj);

            if (obj is IPoolable poolable) poolable.Reset();
        }
    }
}