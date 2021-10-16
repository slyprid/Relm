using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;

namespace Relm.UI.Controls
{
    public abstract class Control
        : IControl
    {
        public Vector2 Size { get; set; }
        public virtual Vector2 Position { get; set; }
        public KeyboardStateExtended KeyboardState { get; set; }
        public MouseStateExtended MouseState { get; set; }
        public Vector2 Scale { get; set; }
        public GameScreen ParentScreen { get; set; }
        public bool IsVisible { get; set; }

        public int Width => (int)Size.X;
        public int Height => (int)Size.Y;
        public int X => (int)Position.X;
        public int Y => (int)Position.Y;
        public Rectangle Bounds => new Rectangle(X, Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));
        
        public virtual void Configure() { }
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public virtual void Update(GameTime gameTime)
        {
            MouseState = Input.GetMouseState();
        }

        protected Control()
        {
            Scale = Vector2.One;
            IsVisible = true;
        }

        #region Fluent Functions

        public T As<T>()
            where T : IControl
        {
            return (T) Convert.ChangeType(this, typeof(T));
        }

        public T SetPosition<T>(int x, int y)
            where T : IControl
        {
            Position = new Vector2(x, y);
            return (T) Convert.ChangeType(this, typeof(T));
        }

        public T SetPosition<T>(Vector2 position)
            where T : IControl
        {
            Position = position;
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T SetScale<T>(float scale)
            where T : IControl
        {
            Scale = new Vector2(scale, scale);
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T SetScale<T>(float scaleX, float scaleY)
            where T : IControl
        {
            Scale = new Vector2(scaleX, scaleY);
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T SetSize<T>(int width, int height)
            where T : IControl
        {
            Size = new Vector2(width, height);
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T Offset<T>(int x, int y)
            where T : IControl
        {
            Position += new Vector2(x, y);
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T Offset<T>(Vector2 offset)
            where T : IControl
        {
            Position += offset;
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public T SetVisibility<T>(bool value)
        {
            IsVisible = value;
            return (T)Convert.ChangeType(this, typeof(T));
        }

        #endregion
    }
}