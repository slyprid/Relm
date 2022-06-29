using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;

namespace Relm.Sprites
{
    public class Sprite
        : Entity
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
    }
}