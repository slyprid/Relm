using System;
using Relm.Constants;
using Relm.Sprites;

namespace Relm.UI.Controls
{
    public abstract class Control
        : Sprite
    {
        public override string Name { get; }

        public abstract Skin Skin { get; }

        protected Control()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
        }

        protected Control(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
        }

        public abstract void InitializeEvents();
    }
}