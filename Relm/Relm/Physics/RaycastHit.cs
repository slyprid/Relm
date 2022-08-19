using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;

namespace Relm.Physics
{
    public struct RaycastHit
    {
        public Collider Collider;
        public float Fraction;
        public float Distance;
        public Vector2 Point;
        public Vector2 Normal;
        public Vector2 Centroid;

        public RaycastHit(Collider collider, float fraction, float distance, Vector2 point, Vector2 normal)
        {
            Collider = collider;
            Fraction = fraction;
            Distance = distance;
            Point = point;
            Normal = normal;
            Centroid = Vector2.Zero;
        }

        internal void SetValues(Collider collider, float fraction, float distance, Vector2 point)
        {
            Collider = collider;
            Fraction = fraction;
            Distance = distance;
            Point = point;
        }

        internal void SetValues(float fraction, float distance, Vector2 point, Vector2 normal)
        {
            Fraction = fraction;
            Distance = distance;
            Point = point;
            Normal = normal;
        }

        internal void Reset()
        {
            Collider = null;
            Fraction = Distance = 0f;
        }

        public override string ToString()
        {
            return $"[RaycastHit] fraction: {Fraction}, distance: {Distance}, normal: {Normal}, centroid: {Centroid}, point: {Point}";
        }
    }
}