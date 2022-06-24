using System.Collections.ObjectModel;

namespace Relm.Extensions
{
    public static class ObservableCollectionExtensions
    {
        public static ObservableCollection<T> Clone<T>(this ObservableCollection<T> collection)
        {
            var ret = new ObservableCollection<T>();

            foreach (var item in collection)
            {
                var clone = (T) item;
                ret.Add(clone);
            }

            return ret;
        }

        public static ObservableCollection<T> Clone<T>(this ObservableCollection<T> collection, ObservableCollection<T> otherCollection)
        {
            collection.Clear();
            foreach (var item in otherCollection)
            {
                var clone = (T)item;
                collection.Add(clone);
            }

            return collection;
        }
    }
}