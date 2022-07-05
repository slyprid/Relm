using System;
using System.Collections.Generic;
using System.Linq;

namespace Relm.Extensions
{
    public static class ListExtensions
    {
        public static List<T> Get<T>(this List<T> items, Func<dynamic, bool> query)
        {
            return items.Where(item => query.Invoke(item)).ToList();
        }

        public static List<TOut> Get<T, TOut>(this List<T> items, Func<dynamic, bool> query)
        {
            return items.Where(item => query.Invoke(item)).Cast<TOut>().ToList();
        }
    }
}