namespace Relm.Core.Managers
{
	public interface IManaged<T>
	{
		IManager<T> Manager { get; set; }
		string MyAlias { get; set; }
	}
}