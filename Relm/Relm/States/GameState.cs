﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Input;
using Relm.Scenes;
using Relm.Sprites;
using Relm.UI;

namespace Relm.States
{
    public static partial class GameState
    {
        public static int DebugValue { get; set; }
        public static ContentManager Content { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static SceneManager Scenes { get; set; }
        public static InputManager Input { get; set; }
        public static UserInterfaceSettings UserInterfaceSettings { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }
        public static Dictionary<string, SpriteFont> Fonts { get; set; }
        public static Dictionary<string, SpriteSheet> SpriteSheets { get; set; }

        #region Initialization

        public static void Initialize()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
            SpriteSheets = new Dictionary<string, SpriteSheet>();
            Scenes = new SceneManager();
            Input = new InputManager();
        }

        public static void InitializeRelm()
        {
            LoadTextures();
            LoadFonts();
        }

        #endregion

        #region Load Methods

        public static void LoadTexture(string name, string assetName)
        {
            Textures.Add(name, Content.Load<Texture2D>(assetName));
        }

        public static void LoadTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        public static void LoadFont(string name, string assetName)
        {
            Fonts.Add(name, Content.Load<SpriteFont>(assetName));
        }

        public static SpriteSheet LoadSpriteSheet(string name, string textureName, int width, int height)
        {
            var ret = new SpriteSheet(textureName, width, height);
            SpriteSheets.Add(name, ret);
            return ret;
        }

        public static SpriteSheet LoadSpriteSheet(string name, string textureName, Dictionary<string, Rectangle> atlas)
        {
            var ret = new SpriteSheet(textureName, atlas);
            SpriteSheets.Add(name, ret);
            return ret;
        }

        #endregion

        #region Relm Load Methods

        private static void LoadTextures()
        {
            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            LoadTexture(TextureNames.Pixel, pixel);

            LoadTexture(TextureNames.Logo, "gfx/relm/ForgottenStarStudiosLogo");
            LoadTexture(TextureNames.UserInterfaceSkin, "gfx/relm/UserInterfaceSkin");
        }

        private static void LoadFonts()
        {
            LoadFont(FontNames.Default, "fonts/Default");
        }

        public static void LoadScenes<T>()
        {
            var sceneTypes = typeof(T).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Scene)));

            foreach (var sceneType in sceneTypes)
            {
                var scene = (Scene)Activator.CreateInstance(sceneType);
                scene.SpriteBatch = new SpriteBatch(GraphicsDevice);
                Scenes.AddScene(scene.Name, scene);
            }
        }

        public static void LoadUserInterface<T>()
        {
            var settingTypes = typeof(T).Assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(UserInterfaceSettings)));
            var settingType = settingTypes.FirstOrDefault();
            if (settingType == null) return;
            UserInterfaceSettings = (UserInterfaceSettings)Activator.CreateInstance(settingType);
            UserInterfaceSettings.Initialize(Content);
        }

        #endregion
    }
}