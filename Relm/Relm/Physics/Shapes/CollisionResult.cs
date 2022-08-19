using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;

namespace Relm.Physics.Shapes
{
    public struct CollisionResult
    {
        public Collider Collider;

        public Vector2 Normal;

        public Vector2 MinimumTranslationVector;

        public Vector2 Point;

        public void RemoveHorizontalTranslation(Vector2 deltaMovement)
        {
            if (System.Math.Sign(Normal.X) != System.Math.Sign(deltaMovement.X) || deltaMovement.X == 0f && Normal.X != 0f)
            {
                var responseDistance = MinimumTranslationVector.Length();
                var fix = responseDistance / Normal.Y;

                if (System.Math.Abs(Normal.X) != 1f && System.Math.Abs(fix) < System.Math.Abs(deltaMovement.Y * 3f))
                {
                    MinimumTranslationVector = new Vector2(0f, -fix);
                }
            }
        }

        public void InvertResult()
        {
            Vector2.Negate(ref MinimumTranslationVector, out MinimumTranslationVector);
            Vector2.Negate(ref Normal, out Normal);
        }

        public override string ToString()
        {
            return string.Format("[CollisionResult] normal: {0}, minimumTranslationVector: {1}", Normal,
                MinimumTranslationVector);
        }
    }
}