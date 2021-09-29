using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Relm
{
    public static class ContentLibrary
    {
        public static ContentManager Content { get; set; }

        public static ContentLibrarySection<string, Texture2D> Textures { get; private set; }
        public static ContentLibrarySection<string, SpriteFont> Fonts { get; private set; }

        public static void Initialize()
        {
            Textures = new ContentLibrarySection<string, Texture2D>(x => x.Name, Content);
            Fonts = new ContentLibrarySection<string, SpriteFont>(x => x.Texture.Name, Content);
        }
    }
}