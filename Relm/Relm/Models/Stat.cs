using System.Runtime.CompilerServices;

namespace Relm.Models
{
    public class Stat<T>
    {
        public T Minimum { get; set; }
        public T Maximum { get; set; }
        public T Value { get; set; }
        public T AdjustedMaximum { get; set; }

        public Stat() { }
        
        public Stat(T minimum, T maximum, T value)
        {
            Minimum = minimum;
            Maximum = maximum;
            Value = value;
        }

        public Stat(T minimum, T maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
            Value = Maximum;
        }
    }
}