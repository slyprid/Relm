using Relm.Collections;
using Relm.Components.Physics.Colliders;
using Relm.Entities;
using System.Collections.Generic;
using Relm.Components.Physics;

namespace Relm.Physics
{
    public class ColliderTriggerHelper
    {
        Entity _entity;
        private HashSet<Pair<Collider>> _activeTriggerIntersections = new();
        HashSet<Pair<Collider>> _previousTriggerIntersections = new();
        List<ITriggerListener> _tempTriggerList = new();

        public ColliderTriggerHelper(Entity entity)
        {
            _entity = entity;
        }
        
        public void Update()
        {
            var colliders = _entity.GetComponents<Collider>();
            for (var i = 0; i < colliders.Count; i++)
            {
                var collider = colliders[i];

                var neighbors = RelmPhysics.BoxcastBroadphase(collider.Bounds, collider.CollidesWithLayers);
                foreach (var neighbor in neighbors)
                {
                    if (!collider.IsTrigger && !neighbor.IsTrigger) continue;

                    if (collider.Overlaps(neighbor))
                    {
                        var pair = new Pair<Collider>(collider, neighbor);

                        var shouldReportTriggerEvent = !_activeTriggerIntersections.Contains(pair) && !_previousTriggerIntersections.Contains(pair);
                        if (shouldReportTriggerEvent) NotifyTriggerListeners(pair, true);

                        _activeTriggerIntersections.Add(pair);
                    }
                }
            }

            ListPool<Collider>.Free(colliders);

            CheckForExitedColliders();
        }


        void CheckForExitedColliders()
        {
            _previousTriggerIntersections.ExceptWith(_activeTriggerIntersections);

            foreach (var pair in _previousTriggerIntersections)
                NotifyTriggerListeners(pair, false);

            _previousTriggerIntersections.Clear();

            _previousTriggerIntersections.UnionWith(_activeTriggerIntersections);

            _activeTriggerIntersections.Clear();
        }


        void NotifyTriggerListeners(Pair<Collider> collisionPair, bool isEntering)
        {
            collisionPair.First.Entity.GetComponents(_tempTriggerList);
            for (var i = 0; i < _tempTriggerList.Count; i++)
            {
                if (isEntering)
                    _tempTriggerList[i].OnTriggerEnter(collisionPair.Second, collisionPair.First);
                else
                    _tempTriggerList[i].OnTriggerExit(collisionPair.Second, collisionPair.First);
            }

            _tempTriggerList.Clear();

            if (collisionPair.Second.Entity != null)
            {
                collisionPair.Second.Entity.GetComponents(_tempTriggerList);
                for (var i = 0; i < _tempTriggerList.Count; i++)
                {
                    if (isEntering)
                        _tempTriggerList[i].OnTriggerEnter(collisionPair.First, collisionPair.Second);
                    else
                        _tempTriggerList[i].OnTriggerExit(collisionPair.First, collisionPair.Second);
                }

                _tempTriggerList.Clear();
            }
        }
    }
}