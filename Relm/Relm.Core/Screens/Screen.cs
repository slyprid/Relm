using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core.Managers;

namespace Relm.Core.States
{
    public abstract class Screen
    {
        public string MyAlias { get; set; }

        public static string GenerateAlias()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual void Update(GameTime gameTime) { }

        public virtual void Draw(GameTime gameTime) { }
    }
}