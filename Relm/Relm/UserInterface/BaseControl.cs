using System;
using Microsoft.Xna.Framework;
using Relm.Sprites;
using Relm.Textures;

namespace Relm.UserInterface
{
    public abstract class BaseControl
        : Sprite, IControl
    {
        public BaseControl ParentControl { get; set; }
        public override Vector2 ParentPosition => ParentControl?.Position ?? new Vector2(float.MinValue, float.MinValue);

        protected BaseControl(TextureAtlas skin)
        {
            TextureAtlas = skin;
        }

        public new T AddChild<T>(params object[] args)
            where T : BaseControl
        {
            var child = (T)Activator.CreateInstance(typeof(T), args);
            child.ParentControl = this;
            Children.Add(child);
            return child;
        }
    }
}