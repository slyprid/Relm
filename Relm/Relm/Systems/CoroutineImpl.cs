using Relm.Collections;
using System.Collections;

namespace Relm.Systems
{
    internal class CoroutineImpl
        : ICoroutine, IPoolable
    {
        public IEnumerator Enumerator;

        public float WaitTimer;

        public bool IsDone;
        public CoroutineImpl WaitForCoroutine;
        public bool UseUnscaledDeltaTime = false;
        
        public void Stop()
        {
            IsDone = true;
        }

        public ICoroutine SetUseUnscaledDeltaTime(bool useUnscaledDeltaTime)
        {
            UseUnscaledDeltaTime = useUnscaledDeltaTime;
            return this;
        }

        internal void PrepareForReuse()
        {
            IsDone = false;
        }

        void IPoolable.Reset()
        {
            IsDone = true;
            WaitTimer = 0;
            WaitForCoroutine = null;
            Enumerator = null;
            UseUnscaledDeltaTime = false;
        }
    }
}