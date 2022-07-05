using Microsoft.Xna.Framework;
// ReSharper disable PossibleNullReferenceException

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
            if (_rnd == null) Initialize();
            return _rnd.Next(maxValue);
        }

        public static int Next(int minValue, int maxValue)
        {
            if (_rnd == null) Initialize();
            return _rnd.Next(minValue, maxValue);
        }

        public static double NextDouble()
        {
            if (_rnd == null) Initialize();
            return _rnd.NextDouble();
        }

        public static float NextFloat()
        {
            if (_rnd == null) Initialize();
            return (float)_rnd.NextDouble();
        }

        public static Color NextColor()
        {
            return new Color(NextFloat(), NextFloat(), NextFloat());
        }
    }
}