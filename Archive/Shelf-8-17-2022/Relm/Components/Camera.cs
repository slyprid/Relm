using Microsoft.Xna.Framework;

namespace Relm.Components
{
    public class Camera
        : Component
    {
        public Vector2 Position
        {
            get => Entity.Transform.Position;
            set => Entity.Transform.Position = value;
        }
    }
}