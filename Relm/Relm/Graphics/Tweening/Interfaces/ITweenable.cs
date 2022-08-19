namespace Relm.Graphics.Tweening.Interfaces
{
    public interface ITweenable
    {
        bool Tick();
        void RecycleSelf();
        bool IsRunning();
        void Start();
        void Pause();
        void Resume();
        void Stop(bool bringToCompletion = false);
    }
}