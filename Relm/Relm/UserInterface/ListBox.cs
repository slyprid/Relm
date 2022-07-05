using System;
using System.Collections.Generic;
using System.Linq;
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
        private string _borderName;

        public List<ListBoxItem> Items { get; set; }
        public Color StartBackgroundColor { get; set; }
        public Color EndBackgroundColor { get; set; }
        public int LeftRightPadding { get; set; }
        public int TopBottomPadding { get; set; }
        public int TopIndex { get; set; }

        public ListBox(TextureAtlas skin) 
            : base(skin)
        {
            Items = new List<ListBoxItem>();
            StartBackgroundColor = Color.Transparent;
            EndBackgroundColor = Color.Transparent;
            LeftRightPadding = 24;
            TopBottomPadding = 32;
            _scrollBarName = Guid.NewGuid().ToString();
            _borderName = Guid.NewGuid().ToString();

            Initialize();
        }

        private void Initialize()
        {
            Children.Clear();

            AddChild<Border>(TextureAtlas)
                .WithName<Border>(_borderName)
                .WithSize(Size)
                .WithBackgroundColor(StartBackgroundColor, EndBackgroundColor);

            AddChild<VerticalScrollBar>(TextureAtlas)
                .WithName(_scrollBarName)
                .WithPosition<VerticalScrollBar>(Width - UserInterfaceSkin.VerticalScrollBarWidth - LeftRightPadding, TopBottomPadding)
                .WithSize(UserInterfaceSkin.VerticalScrollBarWidth, Height - (TopBottomPadding * 2));
        }

        public void Increment(int value = 1)
        {
            var scrollBar = GetChild<VerticalScrollBar>(_scrollBarName);
            scrollBar.Increment(value);
        }

        public void Decrement(int value = 1)
        {
            var scrollBar = GetChild<VerticalScrollBar>(_scrollBarName);
            scrollBar.Decrement(value);
        }

        public void AddItem(ListBoxItem item)
        {
            var border = GetChild<Border>(_borderName);
            border.WithDrawOn(DrawItems);
            item.Parent = this;
            item.Index = Items.Count;
            item.WithPositionOffset(0f, item.Height * item.Index);
            Items.Add(item);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }

        public void DrawItems(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Items.Count == 0) return;

            //Items.ForEach(x => x.Draw(gameTime, spriteBatch));

            var itemHeight = Items.First().Height;
            var bottomIndex = (Height / itemHeight) + TopIndex;

            for (var i = TopIndex; i < bottomIndex; i++)
            {
                var item = Items[i];
                item.Draw(gameTime, spriteBatch);
            }

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