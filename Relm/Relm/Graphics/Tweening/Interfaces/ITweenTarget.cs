namespace Relm.Graphics.Tweening.Interfaces
{
    public interface ITweenTarget<T> 
        where T : struct
    {
        void SetTweenedValue(T value);
        T GetTweenedValue();
        object GetTargetObject();
    }
}