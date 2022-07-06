using System;
using Microsoft.Xna.Framework;
using Relm.Sprites;
using Relm.Textures;

namespace Relm.UserInterface
{
    public abstract class BaseControl
        : Sprite, IControl
    {
        public TextureAtlas Skin => TextureAtlas;

        protected BaseControl(TextureAtlas skin)
        {
            TextureAtlas = skin;
        }

        public new T AddChild<T>(params object[] args)
            where T : BaseControl
        {
            var child = (T)Activator.CreateInstance(typeof(T), args);
            child.Parent = this;
            Children.Add(child);
            return child;
        }
    }
}