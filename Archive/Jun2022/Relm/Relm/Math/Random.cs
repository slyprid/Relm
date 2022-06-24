using Microsoft.Xna.Framework;

namespace Relm.Math
{
    public static class Random
    {
        private static System.Random _rnd;

        public static void Initialize()
        {
            _rnd = new System.Random();
        }

        public static int Next(int maxValue)
        {
            return _rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            return _rnd.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            return _rnd.NextDouble();
        }

        public static float NextFloat()
        {
            return (float) _rnd.NextDouble();
        }

        public static Color NextColor()
        {
            return new Color(NextFloat(), NextFloat(), NextFloat());
        }
    }
}