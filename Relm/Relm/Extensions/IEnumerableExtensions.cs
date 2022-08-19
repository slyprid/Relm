using System.Collections;
using System.Collections.Generic;

namespace Relm.Extensions
{
    public static class IEnumerableExtensions
    {
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            Assert.IsNotNull(source, "source cannot be null");

            // Optimization for ICollection<T> 
            var genericCollection = source as ICollection<TSource>;
            if (genericCollection != null)
                return genericCollection.Count;

            // Optimization for ICollection 
            var nonGenericCollection = source as ICollection;
            if (nonGenericCollection != null)
                return nonGenericCollection.Count;

            // Do it the slow way - and make sure we overflow appropriately 
            checked
            {
                int count = 0;
                using (var iterator = source.GetEnumerator())
                {
                    while (iterator.MoveNext())
                        count++;
                }

                return count;
            }
        }
    }
}