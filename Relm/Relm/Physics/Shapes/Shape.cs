using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Math;

namespace Relm.Physics.Shapes
{
    public abstract class Shape
    {
        internal Vector2 position;

        internal Vector2 center;

        internal RectangleF bounds;


        internal abstract void RecalculateBounds(Collider collider);

        public abstract bool Overlaps(Shape other);

        public abstract bool CollidesWithShape(Shape other, out CollisionResult result);

        public abstract bool CollidesWithLine(Vector2 start, Vector2 end, out RaycastHit hit);

        public abstract bool ContainsPoint(Vector2 point);

        public abstract bool PointCollidesWithShape(Vector2 point, out CollisionResult result);


        public virtual Shape Clone()
        {
            return MemberwiseClone() as Shape;
        }
    }
}