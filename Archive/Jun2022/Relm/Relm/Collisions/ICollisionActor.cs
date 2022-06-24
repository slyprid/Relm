using Microsoft.Xna.Framework;

namespace Relm.Collisions
{
    public interface ICollisionActor
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }

        Rectangle CollisionBounds { get; }
        bool IsColliding { get; set; }

        void OnCollision(ICollider collider);
    }
}