using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Relm.UI.Configuration;

namespace Relm.UI
{
    public abstract class UserInterfaceSkin
    {
        public Dictionary<Type, IConfig> ControlConfigurations { get; internal set; }
        public Texture2D Texture { get; set; }

        protected UserInterfaceSkin()
        {
            ControlConfigurations = new Dictionary<Type, IConfig>();
        }

        public TConfig Add<T, TConfig>()
            where T : IControl
            where TConfig : IConfig
        {
            var config = Activator.CreateInstance<TConfig>();
            ControlConfigurations.Add(typeof(T), config);
            return config;
        }
    }
}