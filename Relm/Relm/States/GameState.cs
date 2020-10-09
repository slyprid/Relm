using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;

namespace Relm.States
{
    public static partial class GameState
    {
        public static ContentManager Content { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
        public static Dictionary<string, Texture2D> Textures { get; set; }
        public static Dictionary<string, SpriteFont> Fonts { get; set; }

        #region Initialization

        public static void Initialize()
        {
            Textures = new Dictionary<string, Texture2D>();
            Fonts = new Dictionary<string, SpriteFont>();
        }

        public static void InitializeRelm()
        {
            LoadTextures();
        }

        #endregion

        #region Load Methods

        public static void LoadTexture(string name, string assetName)
        {
            Textures.Add(name, Content.Load<Texture2D>(assetName));
        }

        private static void LoadTexture(string name, Texture2D texture)
        {
            Textures.Add(name, texture);
        }

        private static void LoadFont(string name, string assetName)
        {
            Fonts.Add(name, Content.Load<SpriteFont>(assetName));
        }

        #endregion

        #region Relm Load Methods

        private static void LoadTextures()
        {
            var pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });
            LoadTexture(TextureNames.Pixel, pixel);
        }

        #endregion
    }
}