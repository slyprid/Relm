using System;
using System.Collections.Generic;

namespace Relm.TimeElements
{
    public static class TimeElementParts
    {
        public static string BackExtra = nameof(BackExtra);
        public static string BackHair = nameof(BackHair);
        public static string Bottom = nameof(Bottom);
        public static string FrontExtra = nameof(FrontExtra);
        public static string Hair = nameof(Hair);
        public static string Hat = nameof(Hat);
        public static string Head = nameof(Head);
        public static string Shadow = nameof(Shadow);
        public static string Top = nameof(Top);
        public static string Weapon = nameof(Weapon);

        private static readonly List<string> Parts = new List<string>
        {
            BackExtra,
            BackHair,
            Bottom,
            FrontExtra,
            Hair,
            Hat,
            Head,
            Shadow,
            Top,
            Weapon
        };

        public static void ForEach(Action<string> action)
        {
            Parts.ForEach(action);
        }
    }
}