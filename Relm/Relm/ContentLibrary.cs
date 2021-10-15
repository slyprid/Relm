using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Fonts;

namespace Relm
{
    public static class ContentLibrary
    {
        public static ContentManager Content { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }

        public static ContentLibrarySection<string, Texture2D> Textures { get; private set; }
        public static ContentLibrarySection<string, SpriteFont> Fonts { get; private set; }
        public static ContentLibrarySection<string, FontSet> FontSets { get; private set; }

        public static void Initialize()
        {
            Textures = new ContentLibrarySection<string, Texture2D>(x => x.Name, (value, key) => { value.Name = key; return value; }, Content);
            Fonts = new ContentLibrarySection<string, SpriteFont>(x => x.Texture.Name, Content);
            FontSets = new ContentLibrarySection<string, FontSet>(x => x.Name, Content);
        }
    }
}