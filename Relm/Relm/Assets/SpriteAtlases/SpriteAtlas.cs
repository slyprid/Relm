using System;
using Relm.Graphics.Textures;

namespace Relm.Assets.SpriteAtlases
{
    public class SpriteAtlas : IDisposable
    {
        public string[] Names;
        public Sprite[] Sprites;

        public string[] AnimationNames;
        public SpriteAnimation[] SpriteAnimations;

        public Sprite GetSprite(string name)
        {
            var index = Array.IndexOf(Names, name);
            return Sprites[index];
        }

        public bool ContainsSprite(string name)
        {
            var index = Array.IndexOf(Names, name);
            return index != -1;
        }

        public SpriteAnimation GetAnimation(string name)
        {
            var index = Array.IndexOf(AnimationNames, name);
            return SpriteAnimations[index];
        }

        void IDisposable.Dispose()
        {
            if (Sprites != null)
            {
                Sprites[0].Texture2D.Dispose();
                Sprites = null;
            }
        }
    }
}