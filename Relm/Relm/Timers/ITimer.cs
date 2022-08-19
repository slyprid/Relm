namespace Relm.Timers
{
    public interface ITimer
    {
        object Context { get; }
        void Stop();
        void Reset();
        T GetContext<T>();
    }
}