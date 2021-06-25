using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Collections;

namespace Relm
{
    public static class ContentLibrary
    {
        public static ContentManager Content { get; set; }

        public static ContentLibrarySection<string, Texture2D> Textures { get; private set; }

        public static void Initialize()
        {
            Textures = new ContentLibrarySection<string, Texture2D>(x => x.Name, Content);
        }
    }
}