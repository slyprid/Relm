using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics
{
    public abstract class ViewportAdapter
        : IDisposable
    {
        public abstract int VirtualWidth { get; }
        public abstract int VirtualHeight { get; }
        public abstract int ViewportWidth { get; }
        public abstract int ViewportHeight { get; }

        public GraphicsDevice GraphicsDevice { get; }
        public Viewport Viewport => GraphicsDevice.Viewport;
        public Rectangle Bounds => new Rectangle(0, 0, VirtualWidth, VirtualHeight);
        public Point Center => Bounds.Center;
        public Matrix TransformMatrix => GetScaleMatrix();

        protected ViewportAdapter(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public void Dispose() { }
        public virtual void Reset() { }

        public abstract Matrix GetScaleMatrix();

        public Point PointToScreen(Point point)
        {
            return PointToScreen(point.X, point.Y);
        }

        public Vector2 VectorToScreen(Vector2 input)
        {
            var scaleMatrix = GetScaleMatrix();
            var invertedMatrix = Matrix.Invert(scaleMatrix);
            return Vector2.Transform(input, invertedMatrix);
        }

        public virtual Point PointToScreen(int x, int y)
        {
            return VectorToScreen(new Vector2(x, y)).ToPoint();
        }
    }
}