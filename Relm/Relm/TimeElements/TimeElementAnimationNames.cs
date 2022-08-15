using System;
using System.Collections.Generic;

namespace Relm.TimeElements
{
    public static class TimeElementAnimationNames
    {
        public static string Walk = nameof(Walk);
        public static string ArmsUp = nameof(ArmsUp);
        public static string BowWalk = nameof(BowWalk);
        public static string Climb = nameof(Climb);
        public static string BowNock = nameof(BowNock);
        public static string Crouch = nameof(Crouch);
        public static string Jump = nameof(Jump);
        public static string WindUp = nameof(WindUp);
        public static string Attack = nameof(Attack);
        public static string Dead = nameof(Dead);

        private static readonly List<string> Names = new List<string>
        {
            Walk,
            ArmsUp,
            BowWalk,
            Climb,
            BowNock,
            Crouch,
            Jump,
            WindUp,
            Attack,
            Dead
        };

        public static void ForEach(Action<string> action)
        {
            Names.ForEach(action);
        }
    }
}