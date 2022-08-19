using System;
using System.Linq.Expressions;

namespace Relm.Animations
{
    public abstract class TweenMember<T> 
        : TweenMember
            where T : struct
    {
        public static Func<T, T, T> Add { get; }
        public static Func<T, T, T> Subtract { get; }
        public static Func<T, float, T> Multiply { get; }

        static TweenMember()
        {
            var a = Expression.Parameter(typeof(T));
            var b = Expression.Parameter(typeof(T));
            var c = Expression.Parameter(typeof(float));
            Add = Expression.Lambda<Func<T, T, T>>(Expression.Add(a, b), a, b).Compile();
            Subtract = Expression.Lambda<Func<T, T, T>>(Expression.Subtract(a, b), a, b).Compile();
            Multiply = Expression.Lambda<Func<T, float, T>>(Expression.Multiply(a, c), a, c).Compile();
        }

        protected TweenMember(object target, Func<object, object> getMethod, Action<object, object> setMethod)
            : base(target)
        {
            _getMethod = getMethod;
            _setMethod = setMethod;
        }

        private readonly Func<object, object> _getMethod;
        private readonly Action<object, object> _setMethod;

        public T Value
        {
            get => (T)_getMethod(Target);
            set => _setMethod(Target, value);
        }
    }
}