using System;

namespace Relm.Graphics.Tweening.Interfaces
{
	public interface ITween<T> 
        : ITweenControl where T : struct
	{
		ITween<T> SetEaseType(EaseType easeType);
        ITween<T> SetDelay(float delay);
        ITween<T> SetDuration(float duration);
        ITween<T> SetTimeScale(float timeScale);
        ITween<T> SetIsTimeScaleIndependent();
        ITween<T> SetCompletionHandler(Action<ITween<T>> completionHandler);
        ITween<T> SetLoops(LoopType loopType, int loops = 1, float delayBetweenLoops = 0f);
        ITween<T> SetLoopCompletionHandler(Action<ITween<T>> loopCompleteHandler);
        ITween<T> SetFrom(T from);
        ITween<T> PrepareForReuse(T from, T to, float duration);
        ITween<T> SetRecycleTween(bool shouldRecycleTween);
        ITween<T> SetIsRelative();
        ITween<T> SetContext(object context);
        ITween<T> SetNextTween(ITweenable nextTween);
	}
}