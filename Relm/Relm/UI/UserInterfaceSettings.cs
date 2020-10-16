using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Relm.UI
{
    public abstract class UserInterfaceSettings
    {
        public Dictionary<Type, Skin> Skins { get; set; }

        protected UserInterfaceSettings()
        {
            Skins = new Dictionary<Type, Skin>();
        }

        public abstract void Initialize(ContentManager content);

        public Skin AddSkinFor<T>()
        {
            var skin = new Skin
            {
                SkinType = typeof(T)
            };

            Skins.Add(typeof(T), skin);

            return skin;
        }
    }
}