using Relm.Collections;
using Relm.Components.Physics.Colliders;
using Relm.Physics.Shapes;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Physics;

namespace Relm.Components.Physics.Movers
{
    public class Mover : Component
    {
        ColliderTriggerHelper _triggerHelper;

        public override void OnAddedToEntity()
        {
            _triggerHelper = new ColliderTriggerHelper(Entity);
        }

        public bool CalculateMovement(ref Vector2 motion, out CollisionResult collisionResult)
        {
            collisionResult = new CollisionResult();

            if (Entity.GetComponent<Collider>() == null || _triggerHelper == null) return false;

            var colliders = Entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];

                if (collider.IsTrigger) continue;

                var bounds = collider.Bounds;
                bounds.X += motion.X;
                bounds.Y += motion.Y;
                var neighbors = RelmPhysics.BoxcastBroadphaseExcludingSelf(collider, ref bounds, collider.CollidesWithLayers);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.IsTrigger)
                        continue;

                    if (collider.CollidesWith(neighbor, motion, out CollisionResult _InternalcollisionResult))
                    {
                        motion -= _InternalcollisionResult.MinimumTranslationVector;

                        if (_InternalcollisionResult.Collider != null) collisionResult = _InternalcollisionResult;
                    }
                }
            }

            ListPool<Collider>.Free(colliders);

            return collisionResult.Collider != null;
        }

        public int AdvancedCalculateMovement(ref Vector2 motion, ICollection<CollisionResult> collisionResults)
        {
            int Collisions = 0;
            if (Entity.GetComponent<Collider>() == null || _triggerHelper == null) return Collisions;

            var colliders = Entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];

                if (collider.IsTrigger)
                    continue;

                var bounds = collider.Bounds;
                bounds.X += motion.X;
                bounds.Y += motion.Y;
                var neighbors = RelmPhysics.BoxcastBroadphaseExcludingSelf(collider, ref bounds, collider.CollidesWithLayers);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.IsTrigger) continue;

                    if (collider.CollidesWith(neighbor, motion, out CollisionResult _InternalcollisionResult))
                    {
                        motion -= _InternalcollisionResult.MinimumTranslationVector;
                        collisionResults.Add(_InternalcollisionResult);

                        Collisions++;
                    }
                }
            }

            ListPool<Collider>.Free(colliders);

            return Collisions;
        }

        public void ApplyMovement(Vector2 motion)
        {
            Entity.Transform.Position += motion;

            _triggerHelper?.Update();
        }

        public bool Move(Vector2 motion, out CollisionResult collisionResult)
        {
            CalculateMovement(ref motion, out collisionResult);

            ApplyMovement(motion);

            return collisionResult.Collider != null;
        }
    }
}