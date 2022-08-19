using System;
using System.Text;

namespace Relm
{
    public static partial class Utils
    {
        public static string RandomString(int size = 38)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < size; i++)
            {
                var ch = Convert.ToChar(Convert.ToInt32(System.Math.Floor(26 * Math.Random.NextFloat() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

        public static void Swap<T>(ref T first, ref T second)
        {
            T temp = first;
            first = second;
            second = temp;
        }
    }
}