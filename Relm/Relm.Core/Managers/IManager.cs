namespace Relm.Core.Managers
{
	public interface IManager<T>
	{
		void Add(string alias, T item);

		void Remove(string alias);

		void Remove(T item);

		void Clear();

		T GetItem(string alias);
	}
}