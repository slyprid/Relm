using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MonoGame.Extended.Screens.Transitions;
using Relm.UI;

namespace Relm
{
    public static class UserInterface
    {
        internal static UserInterfaceManager UserInterfaceManager { get; set; }
        
        public static Dictionary<string, UserInterfaceScreen> ScreenList { get; }
        public static UserInterfaceSkin Skin { get; internal set; }

        public static UserInterfaceScreen ActiveScreen => UserInterfaceManager.ActiveScreen;

        static UserInterface()
        {
            ScreenList = new Dictionary<string, UserInterfaceScreen>();
        }

        public static bool IsActiveScreen(UserInterfaceScreen screen)
        {
            return ActiveScreen == screen;
        }

        public static void LoadScreen(UserInterfaceScreen screen)
        {
            UserInterfaceManager.LoadScreen(screen);
        }

        public static void LoadScreen(UserInterfaceScreen screen, Transition transition)
        {
            UserInterfaceManager.LoadScreen(screen, transition);
        }

        public static void Add(UserInterfaceScreen screen)
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

        public static UserInterfaceScreen Get(string name)
        {
            return ScreenList[name];
        }

        public static void UseSkin(string skinName, string fontSetName, string fontSetName2)
        {
            var shortName = skinName.Split('/').Last();
            var asm = Assembly.GetCallingAssembly();
            var fullName = $"{asm.FullName.Split(',').First()}.Content.{shortName}";
            Skin = (UserInterfaceSkin)Activator.CreateInstance(asm.GetType(fullName));
            Skin.Texture = ContentLibrary.Textures[skinName];
            Skin.FontSet = ContentLibrary.FontSets[fontSetName];
            Skin.FontSet2 = ContentLibrary.FontSets[fontSetName2];
        }
    }
}