namespace Relm.Systems
{
    public interface ICoroutine
    {
        void Stop();

        ICoroutine SetUseUnscaledDeltaTime(bool useUnscaledDeltaTime);
    }
}