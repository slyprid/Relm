namespace Relm.Animations
{
    public class Tween<T> : Tween
        where T : struct
    {
        private T _startValue;
        private T _endValue;
        private T _range;

        public TweenMember<T> Member { get; }
        public override string MemberName => Member.Name;

        internal Tween(object target, float duration, float delay, TweenMember<T> member, T endValue)
            : base(target, duration, delay)
        {
            Member = member;
            _endValue = endValue;
        }

        protected override void Initialize()
        {
            _startValue = Member.Value;
            _range = TweenMember<T>.Subtract(_endValue, _startValue);
        }

        protected override void Interpolate(float n)
        {
            var value = TweenMember<T>.Add(_startValue, TweenMember<T>.Multiply(_range, n));
            Member.Value = value;
        }

        protected override void Swap()
        {
            _endValue = _startValue;
            Initialize();
        }
    }
}