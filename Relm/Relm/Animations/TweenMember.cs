using System;

namespace Relm.Animations
{
    public abstract class TweenMember
    {
        public object Target { get; }
        public abstract Type Type { get; }
        public abstract string Name { get; }

        protected TweenMember(object target)
        {
            Target = target;
        }
    }
}