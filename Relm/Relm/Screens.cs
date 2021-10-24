using System.Collections.Generic;
using System.Reflection;
using MonoGame.Extended.Screens;
using MonoGame.Extended.Screens.Transitions;
using Relm.Models;

namespace Relm
{
    public static class Screens
    {
        internal static ScreenManager ScreenManager { get; set; }

        public static bool IsPaused { get; private set; }

        public static GameScreen ActiveScreen
        {
            get
            {
                var field = ScreenManager.GetType().GetField("_activeScreen", BindingFlags.NonPublic | BindingFlags.Instance);
                return (GameScreen)field?.GetValue(ScreenManager);
            }
        }

        public static Dictionary<string, RelmGameScreen> ScreenList { get; }

        static Screens()
        {
            ScreenList = new Dictionary<string, RelmGameScreen>();
            IsPaused = false;
        }

        public static bool IsActiveScreen(GameScreen screen)
        {
            return ActiveScreen == screen;
        }

        public static bool IsActiveScreen(string screenName)
        {
            var screen = ScreenList[screenName];
            return IsActiveScreen(screen);
        }

        public static void Pause()
        {
            IsPaused = true;
            ScreenManager.IsEnabled = false;
        }

        public static void UnPause()
        {
            IsPaused = false;
            ScreenManager.IsEnabled = true;
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