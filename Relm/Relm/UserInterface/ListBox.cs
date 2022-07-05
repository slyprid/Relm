using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class ListBox
        : BaseControl
    {
        private string _scrollBarName;

        public List<ListBoxItem> Items { get; set; }
        public Color StartBackgroundColor { get; set; }
        public Color EndBackgroundColor { get; set; }
        public int LeftRightPadding { get; set; }
        public int TopBottomPadding { get; set; }

        public ListBox(TextureAtlas skin) 
            : base(skin)
        {
            Items = new List<ListBoxItem>();
            StartBackgroundColor = Color.Transparent;
            EndBackgroundColor = Color.Transparent;
            LeftRightPadding = 24;
            TopBottomPadding = 32;
            _scrollBarName = Guid.NewGuid().ToString();

            Initialize();
        }

        private void Initialize()
        {
            Children.Clear();

            AddChild<Border>(TextureAtlas)
                .WithSize(Size)
                .WithBackgroundColor(StartBackgroundColor, EndBackgroundColor);

            AddChild<VerticalScrollBar>(TextureAtlas)
                .WithName(_scrollBarName)
                .WithPosition<VerticalScrollBar>(Width - UserInterfaceSkin.VerticalScrollBarWidth - LeftRightPadding, TopBottomPadding)
                .WithSize(UserInterfaceSkin.VerticalScrollBarWidth, Height - (TopBottomPadding * 2));
        }

        public void Increment(int value = 1)
        {
            var scrollBar = Children.Find(x => x.Name == _scrollBarName).As<VerticalScrollBar>();
            scrollBar.Increment(value);
        }

        public void Decrement(int value = 1)
        {
            var scrollBar = Children.Find(x => x.Name == _scrollBarName).As<VerticalScrollBar>();
            scrollBar.Decrement(value);
        }

        #region Fluent Functions

        public ListBox WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            return this;
        }

        public ListBox WithSize(int x, int y)
        {
            return WithSize(new Vector2(x, y));
        }

        public ListBox WithSize(float x, float y)
        {
            return WithSize(new Vector2(x, y));
        }

        public ListBox WithBackgroundColor(Color color)
        {
            StartBackgroundColor = color;
            EndBackgroundColor = color;
            Initialize();
            return this;
        }

        public ListBox WithBackgroundColor(Color startColor, Color endColor)
        {
            StartBackgroundColor = startColor;
            EndBackgroundColor = endColor;
            Initialize();
            return this;
        }

        #endregion
    }
}