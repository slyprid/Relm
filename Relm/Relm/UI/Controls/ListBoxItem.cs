using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using Relm.Extensions;
using Relm.UI.States;
using ButtonState = Relm.UI.States.ButtonState;

namespace Relm.UI.Controls
{
    public class ListBoxItem
    {
        public object Content { get; set; }
        public Vector2 Position { get; set; }
        public SpriteFont Font { get; set; }
        public Color Color { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Scale { get; set; }
        public KeyboardStateExtended KeyboardState { get; set; }
        public MouseStateExtended MouseState { get; set; }
        public int XOffset { get; set; }
        public ListBoxItemState State { get; set; }
        public Action<ListBoxItem> ItemSelected { get; set; }

        public int X => (int)Position.X;
        public int Y => (int)Position.Y;

        public Rectangle Bounds => new Rectangle(X - XOffset, Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));

        public ListBoxItem()
        {
            Scale = Vector2.One;
        }

        public virtual void Update(GameTime gameTime)
        {
            MouseState = Input.GetMouseState();
            
            if(State != ListBoxItemState.Selected) State = ListBoxItemState.Normal;

            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                if (State != ListBoxItemState.Selected) State = ListBoxItemState.Hover;
                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    State = ListBoxItemState.Selected;
                    ItemSelected?.Invoke(this);
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Content == null) return;

            if (State == ListBoxItemState.Hover)
            {
                var pixel = spriteBatch.GetWhitePixel();
                spriteBatch.Draw(pixel, Bounds, new Color(0, 122, 204).WithOpacity(0.5f));
            }
            else if (State == ListBoxItemState.Selected)
            {
                var pixel = spriteBatch.GetWhitePixel();
                spriteBatch.Draw(pixel, Bounds, new Color(0, 122, 204));
            }

            if (Content is string && Font != null)
            {
                spriteBatch.DrawString(Font, Content.ToString(), Position, Color);
            }
        }

        public void CalculateDimensions()
        {
            if (Content == null) return;
            
            if (Content is string && Font != null)
            {
                var size = Font.MeasureString(Content.ToString());
                Height = (int)size.Y;
            }
        }
    }
}