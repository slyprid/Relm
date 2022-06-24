namespace Relm.Tweening
{
    public class DynamicTarget<TTarget>
    {
        public TTarget Value { get; set; }

        public DynamicTarget() { }

        public DynamicTarget(TTarget value)
        {
            Value = value;
        }
    }
}