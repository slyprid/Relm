﻿using Microsoft.Xna.Framework;

namespace Relm.Components.Cameras
{
    public class CameraBounds 
        : Component, IUpdateable
    {
        public Vector2 Min, Max;


        public CameraBounds()
        {
            // make sure we run last so the camera is already moved before we evaluate its position
            SetUpdateOrder(int.MaxValue);
        }


        public CameraBounds(Vector2 min, Vector2 max) : this()
        {
            Min = min;
            Max = max;
        }


        public override void OnAddedToEntity()
        {
            Entity.UpdateOrder = int.MaxValue;
        }


        void IUpdateable.Update()
        {
            var cameraBounds = Entity.Scene.Camera.Bounds;

            if (cameraBounds.Top < Min.Y)
                Entity.Scene.Camera.Position += new Vector2(0, Min.Y - cameraBounds.Top);

            if (cameraBounds.Left < Min.X)
                Entity.Scene.Camera.Position += new Vector2(Min.X - cameraBounds.Left, 0);

            if (cameraBounds.Bottom > Max.Y)
                Entity.Scene.Camera.Position += new Vector2(0, Max.Y - cameraBounds.Bottom);

            if (cameraBounds.Right > Max.X)
                Entity.Scene.Camera.Position += new Vector2(Max.X - cameraBounds.Right, 0);
        }
    }
}