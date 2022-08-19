using System.Collections;

namespace Relm.Graphics.Tweening.Interfaces
{
    public interface ITweenControl 
        : ITweenable
    {
        object Context { get; }
        void JumpToElapsedTime(float elapsedTime);
        IEnumerator WaitForCompletion();
        object GetTargetObject();
    }
}