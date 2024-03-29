﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Fonts;

namespace Relm.Content
{
    public class ContentLibrary
    {
        public ContentManager Content { get; set; }
        
        public ContentLibrarySection<string, Texture2D> Textures { get; private set; }
        public ContentLibrarySection<string, SpriteFont> Fonts { get; private set; }
        public ContentLibrarySection<string, FontSet> FontSets { get; private set; }

        public ContentLibrary(ContentManager content)
        {
            Content = content;
            Textures = new ContentLibrarySection<string, Texture2D>(x => x.Name, (value, key) => { value.Name = key; return value; }, Content);
            Fonts = new ContentLibrarySection<string, SpriteFont>(x => x.Texture.Name, Content);
            FontSets = new ContentLibrarySection<string, FontSet>(x => x.FontName, Content);
        }
    }
}
