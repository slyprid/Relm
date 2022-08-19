using Microsoft.Xna.Framework;

namespace Relm.Components
{
    public class Transform
        : Component
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Vector2 Scale { get; set; } = Vector2.Zero;
        public float LayerDepth { get; set; } = 0;
        public float Rotation { get; set; } = 0;

        public Transform()
        {
            TransformSystem.Register(this);
        }
    }
}