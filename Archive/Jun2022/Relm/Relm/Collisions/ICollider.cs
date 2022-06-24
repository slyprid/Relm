using Microsoft.Xna.Framework;

namespace Relm.Collisions
{
    public interface ICollider
    {
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }

        Rectangle Bounds { get; }
    }
}