using Microsoft.Xna.Framework;

namespace Relm.Physics
{
    public struct Ray2D
    {
        public Vector2 Start;
        public Vector2 End;
        public Vector2 Direction;


        public Ray2D(Vector2 position, Vector2 end)
        {
            Start = position;
            End = end;
            Direction = end - Start;
        }
    }
}