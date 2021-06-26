using System.Collections.Generic;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Relm.Models;

namespace Relm
{
    public static class Screens
    {
        internal static ScreenManager ScreenManager { get; set; }

        public static Dictionary<string, RelmGameScreen> ScreenList { get; }

        static Screens()
        {
            ScreenList = new Dictionary<string, RelmGameScreen>();
        }

        public static void LoadScreen(RelmGameScreen screen)
        {
            ScreenManager.LoadScreen(screen);
        }

        public static void LoadScreen(RelmGameScreen screen, Transition transition)
        {
            ScreenManager.LoadScreen(screen, transition);
        }

        public static void Add(RelmGameScreen screen)
        {
            ScreenList.Add(screen.Name, screen);
        }

        public static void Change(string name)
        {
            LoadScreen(ScreenList[name]);
        }

        public static void Change(string name, Transition transition)
        {
            LoadScreen(ScreenList[name], transition);
        }

        public static RelmGameScreen Get(string name)
        {
            return ScreenList[name];
        }
    }
}