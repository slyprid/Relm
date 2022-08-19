using System.Collections;
using System.Collections.Generic;
using Relm.Collections;
using Relm.Core;

namespace Relm.Systems
{
	public class CoroutineManager 
        : GlobalManager
	{
		private bool _isInUpdate;
        private readonly List<CoroutineImpl> _unblockedCoroutines = new();
		private readonly List<CoroutineImpl> _shouldRunNextFrame = new();


		public void ClearAllCoroutines()
		{
			foreach (var coroutine in _unblockedCoroutines) Pool<CoroutineImpl>.Free(coroutine);

            foreach (var coroutine in _shouldRunNextFrame) Pool<CoroutineImpl>.Free(coroutine);

            _unblockedCoroutines.Clear();
			_shouldRunNextFrame.Clear();
		}

		public ICoroutine StartCoroutine(IEnumerator enumerator)
		{
			var coroutine = Pool<CoroutineImpl>.Obtain();
			coroutine.PrepareForReuse();

			coroutine.Enumerator = enumerator;
			var shouldContinueCoroutine = TickCoroutine(coroutine);

			if (!shouldContinueCoroutine) return null;

			if (_isInUpdate) _shouldRunNextFrame.Add(coroutine);
			else _unblockedCoroutines.Add(coroutine);

			return coroutine;
		}

		public override void Update()
		{
			_isInUpdate = true;
			for (var i = 0; i < _unblockedCoroutines.Count; i++)
			{
				var coroutine = _unblockedCoroutines[i];

				if (coroutine.IsDone)
				{
					Pool<CoroutineImpl>.Free(coroutine);
					continue;
				}

				if (coroutine.WaitForCoroutine != null)
				{
					if (coroutine.WaitForCoroutine.IsDone)
					{
						coroutine.WaitForCoroutine = null;
					}
					else
					{
						_shouldRunNextFrame.Add(coroutine);
						continue;
					}
				}

				if (coroutine.WaitTimer > 0)
				{
					coroutine.WaitTimer -= coroutine.UseUnscaledDeltaTime ? Time.UnscaledDeltaTime : Time.DeltaTime;
					_shouldRunNextFrame.Add(coroutine);
					continue;
				}

				if (TickCoroutine(coroutine))
					_shouldRunNextFrame.Add(coroutine);
			}

			_unblockedCoroutines.Clear();
			_unblockedCoroutines.AddRange(_shouldRunNextFrame);
			_shouldRunNextFrame.Clear();

			_isInUpdate = false;
		}

		private bool TickCoroutine(CoroutineImpl coroutine)
		{
			if (!coroutine.Enumerator.MoveNext() || coroutine.IsDone)
			{
				Pool<CoroutineImpl>.Free(coroutine);
				return false;
			}

			if (coroutine.Enumerator.Current == null)
			{
				return true;
			}

			if (coroutine.Enumerator.Current is WaitForSeconds)
			{
				coroutine.WaitTimer = (coroutine.Enumerator.Current as WaitForSeconds).WaitTime;
				return true;
			}

			if (coroutine.Enumerator.Current is IEnumerator enumerator)
			{
				coroutine.WaitForCoroutine = StartCoroutine(enumerator) as CoroutineImpl;
				return true;
			}

#if DEBUG

			if (coroutine.Enumerator.Current is int)
			{
				Debug.Error("yield Coroutine.waitForSeconds instead of an int. Yielding an int will not work in a release build.");
				coroutine.WaitTimer = (int)coroutine.Enumerator.Current;
				return true;
			}

			if (coroutine.Enumerator.Current is float)
			{
				Debug.Error("yield Coroutine.waitForSeconds instead of a float. Yielding a float will not work in a release build.");
				coroutine.WaitTimer = (float)coroutine.Enumerator.Current;
				return true;
			}
#endif

			if (coroutine.Enumerator.Current is CoroutineImpl)
			{
				coroutine.WaitForCoroutine = coroutine.Enumerator.Current as CoroutineImpl;
				return true;
			}
			else
			{
				return true;
			}
		}
	}
}