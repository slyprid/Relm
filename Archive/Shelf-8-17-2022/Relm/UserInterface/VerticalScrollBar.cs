using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Relm.Naming;
using Relm.Sprites;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class VerticalScrollBar
        : BaseControl
    {
        private readonly List<OldSprite> _slider;
        private int _sliderSize;

        public Color BackgroundColor { get; set; }

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }

        public VerticalScrollBar(TextureAtlas skin)
            : base(skin)
        {
            BackgroundColor = Color.White;

            _slider = new List<OldSprite>();
            Minimum = 0;
            Maximum = 100;
            Value = 0;

            Initialize();
        }

        private void Initialize()
        {
            var w = UserInterfaceSkin.VerticalScrollBarWidth;
            var h = UserInterfaceSkin.VerticalScrollBarHeight;

            Children.Clear();
            _slider.Clear();

            // =======================
            // Bar
            // =======================

            // Top
            AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalScrollBarTop))
                .WithPosition(0, 0);

            int yo;
            for (yo = 0; yo <= Height - (h * 3); yo += h)
            {
                // Middle
                AddChild<ChildControl>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalScrollBarMiddle))
                    .WithPosition(0, h + yo);
            }

            // Bottom
            AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalScrollBarBottom))
                .WithPosition(0, h + yo);

            // =========================
            // Slider
            // =========================

            // Top
            _slider.Add(AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalSliderTop))
                .WithPosition(0, 0));

            // Middle
            _slider.Add(AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalSliderMiddle))
                .WithPosition(0, h));

            // Bottom
            _slider.Add(AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.VerticalSliderBottom))
                .WithPosition(0, h * 2));

            _sliderSize = 32;

            foreach (var child in Children)
            {
                child.Tint = BackgroundColor;
            }
        }

        public void Increment(int value = 1)
        {
            Value += value;
            if (Value >= Maximum)
            {
                Value = Maximum;
            }

            var sy = Position.Y;
            var ey = Position.Y + (Height - _sliderSize);
            var incrementValue = (float)(ey - sy) * ((float)value / (float)Maximum);
            
            foreach (var child in _slider)
            {
                child.Position += new Vector2(0f, incrementValue);
            }

            if (!(_slider[0].Position.Y >= Height - (_sliderSize * 3))) return;
            _slider[0].Position = new Vector2(_slider[0].Position.X, Height - (_sliderSize * 3));
            _slider[1].Position = new Vector2(_slider[1].Position.X, Height - (_sliderSize * 2));
            _slider[2].Position = new Vector2(_slider[2].Position.X, Height - _sliderSize);
        }

        public void Decrement(int value = 1)
        {
            Value -= value;
            if (Value <= Minimum)
            {
                Value = Minimum;
            }

            var sy = Position.Y;
            var ey = Position.Y + (Height - _sliderSize);
            var incrementValue = (float)(ey - sy) * ((float)value / (float)Maximum);
            foreach (var child in _slider)
            {
                child.Position -= new Vector2(0f, incrementValue);
            }

            if (!(_slider[0].Position.Y <= 0)) return;
            _slider[0].Position = new Vector2(_slider[0].Position.X, 0);
            _slider[1].Position = new Vector2(_slider[1].Position.X, _sliderSize);
            _slider[2].Position = new Vector2(_slider[2].Position.X, _sliderSize * 2);
        }

        #region Fluent Functions

        public VerticalScrollBar WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            return this;
        }

        public VerticalScrollBar WithSize(int x, int y)
        {
            return WithSize(new Vector2(x, y));
        }

        public VerticalScrollBar WithSize(float x, float y)
        {
            return WithSize(new Vector2(x, y));
        }

        public VerticalScrollBar WithBackgroundColor(Color color)
        {
            BackgroundColor = color;

            foreach (var child in Children)
            {
                child.Tint = BackgroundColor;
            }

            foreach (var slider in _slider)
            {
                slider.Tint = Color.White;
            }

            return this;
        }

        public VerticalScrollBar WithMinimum(int value)
        {
            Minimum = value;
            return this;
        }

        public VerticalScrollBar WithMaxium(int value)
        {
            Maximum = value;
            return this;
        }

        #endregion
    }
}