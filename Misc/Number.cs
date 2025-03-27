using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace AbstractImagesGenerator.Misc
{
    public record Number(double Value) : IComparable
    {
        public Number(string s) : this(Parse(s)){}

        private static double Parse(string s)
        {
            if (double.TryParse(s.Replace(',', '.'), out var result))
            {
                return result;
            }
            else if (double.TryParse(s.Replace('.', ','), out var _result))
            {
                return _result;
            }
            throw new ArgumentException($"Cannot parse {s} as a number");
        }

        public static explicit operator double(Number n) => n.Value;
        public static explicit operator int(Number n) => (int)n.Value;
        public static implicit operator Number(double i) => new(i);

        public override string ToString() => Value.ToString().Replace(',', '.');

        public int CompareTo(object? obj)
        {
            if (obj is Number n)
            {
                return Value.CompareTo(n.Value);
            }
            double v = Convert.ToDouble(obj);
            return Value.CompareTo(v);
        }
    }
}
