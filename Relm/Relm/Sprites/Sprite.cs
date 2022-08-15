﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Textures;
using Relm.Extensions;
using Relm.UserInterface;

namespace Relm.Sprites
{
    public class Sprite
        : Entity
    {
        private int _width;
        private int _height;

        public Vector2 Position { get; set; }
        public virtual Sprite Parent { get; set; }
        public virtual Texture2D Texture { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public Color Tint { get; set; }
        public string AtlasRegionName { get; set; }
        public List<Sprite> Children { get; set; }
        public Vector2 Scale { get; set; }
        public Action<GameTime, SpriteBatch> DrawOn { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsVisible { get; set; }
        public float Opacity { get; set; }

        public virtual Rectangle Bounds => new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
        public Vector2 Size => new Vector2(Width, Height);

        /// <summary>
        /// Returns width of sprite
        /// If set to -1, will return width of parent
        /// </summary>
        public int Width
        {
            get
            {
                if(_width == -1) return Parent?.Width ?? 0;
                return _width;
            }
            set => _width = value;
        }

        /// <summary>
        /// Returns height of sprite
        /// If set to -1, will return height of parent
        /// </summary>
        public int Height
        {
            get
            {
                if (_height == -1) return Parent?.Height ?? 0;
                return _height;
            }
            set => _height = value;
        }

        public Sprite()
        {
            Tint = Color.White;
            Children = new List<Sprite>();
            Scale = Vector2.One;
            IsEnabled = true;
            IsVisible = true;
            Opacity = 1f;
        }

        public Sprite(Texture2D texture)
            : this()
        {
            Texture = texture;
            Width = texture.Width;
            Height = texture.Height;
        }

        public Sprite(TextureAtlas textureAtlas)
            : this()
        {
            TextureAtlas = textureAtlas;
            var firstRegion = TextureAtlas.Regions.First();
            AtlasRegionName = firstRegion.Name;
            Width = firstRegion.Width;
            Height = firstRegion.Height;
        }

        public Sprite(Texture2D texture, string name)
            : this(texture)
        {
            Name = name;
        }

        public Sprite(TextureAtlas textureAtlas, string name)
            : this(textureAtlas)
        {
            Name = name;
        }

        public virtual void Initialize() { }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled) return;

            base.Update(gameTime);
            Children.ForEach(x => x.Update(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var position = CalculatePosition(this);
            

            if (Texture != null)
            {
                var destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));
                spriteBatch.Draw(Texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            else
            {
                // Checks to see if this is a container sprite
                if (!string.IsNullOrEmpty(AtlasRegionName))
                {
                    var region = TextureAtlas.GetRegion(AtlasRegionName);
                    Width = region.Width;
                    Height = region.Height;
                    spriteBatch.Draw(region, position, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f, null);
                }
            }

            Children.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        protected Vector2 CalculatePosition(Sprite sprite)
        {
            var ret = sprite.Position;

            if (sprite.Parent != null)
            {
                ret += CalculatePosition(sprite.Parent);
            }

            return ret;
        }

        public virtual T AddChild<T>(params object[] args)
            where T : Sprite
        {
            var child = (T)Activator.CreateInstance(typeof(T), args);
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public T GetChild<T>(string name)
            where T : Sprite
        {
            return Children.Find(x => x.Name == name).As<T>();
        }

        #region Fluent Functions

        public virtual Sprite WithAtlasRegionName(string value)
        {
            AtlasRegionName = value;
            return this;
        }

        public virtual T WithAtlasRegionName<T>(string value)
            where T : Sprite
        {
            return (T)WithAtlasRegionName(value);
        }

        public virtual Sprite WithPosition(Vector2 position)
        {
            Position = position;
            return this;
        }

        public virtual T WithPosition<T>(Vector2 position)
            where T : Sprite
        {
            return (T)WithPosition(position);
        }

        public virtual Sprite WithPosition(int x, int y)
        {
            return WithPosition(new Vector2(x, y));
        }

        public virtual T WithPosition<T>(int x, int y)
            where T : Sprite
        {
            return (T)WithPosition(new Vector2(x, y));
        }

        public virtual Sprite WithPosition(float x, float y)
        {
            return WithPosition(new Vector2(x, y));
        }

        public virtual T WithPosition<T>(float x, float y)
            where T : Sprite
        {
            return (T)WithPosition(new Vector2(x, y));
        }

        public virtual Sprite WithPosition(ScreenPosition position)
        {
            switch (position)
            {
                case ScreenPosition.TopLeft:
                    Position = Layout.TopLeft;
                    break;
                case ScreenPosition.TopCenter:
                    Position = Layout.TopCenter - new Vector2(Width / 2f, 0f);
                    break;
                case ScreenPosition.TopRight:
                    Position = Layout.TopRight - new Vector2(Width, 0f);
                    break;
                case ScreenPosition.CenterLeft:
                    Position = Layout.CenterLeft - new Vector2(0f, Height / 2f);
                    break;
                case ScreenPosition.CenterRight:
                    Position = Layout.CenterRight - new Vector2(Width, Height / 2f);
                    break;
                case ScreenPosition.BottomLeft:
                    Position = Layout.BottomLeft - new Vector2(0f, Height * 2);
                    break;
                case ScreenPosition.BottomCenter:
                    Position = Layout.BottomCenter - new Vector2(Width / 2f, Height * 2);
                    break;
                case ScreenPosition.BottomRight:
                    Position = Layout.BottomRight - new Vector2(Width, Height * 2);
                    break;
                default:
                case ScreenPosition.Centered:
                    Position = Layout.Centered(Width, Height);
                    break;
            }

            return this;
        }

        public virtual T WithPosition<T>(ScreenPosition position)
            where T : Sprite
        {
            return (T)WithPosition(position);
        }

        public virtual Sprite WithPositionOffset(Vector2 offset)
        {
            Position += offset;
            return this;
        }

        public virtual T WithPositionOffset<T>(Vector2 offset)
            where T : Sprite
        {
            return (T)WithPositionOffset(offset);
        }

        public virtual Sprite WithPositionOffset(int x, int y)
        {
            return WithPositionOffset(new Vector2(x, y));
        }

        public virtual T WithPositionOffset<T>(int x, int y)
            where T : Sprite
        {
            return (T)WithPositionOffset(new Vector2(x, y));
        }

        public virtual Sprite WithPositionOffset(float x, float y)
        {
            return WithPositionOffset(new Vector2(x, y));
        }

        public virtual T WithPositionOffset<T>(float x, float y)
            where T : Sprite
        {
            return (T)WithPositionOffset(new Vector2(x, y));
        }

        public virtual Sprite WithScale(Vector2 scale)
        {
            Scale = scale;
            Children.ForEach(x => x.Scale = scale);
            return this;
        }

        public virtual T WithScale<T>(Vector2 scale)
            where T : Sprite
        {
            return (T)WithScale(scale);
        }

        public virtual Sprite WithName(string name)
        {
            Name = name;
            return this;
        }

        public virtual T WithName<T>(string name)
            where T : Sprite
        {
            return (T)WithName(name);
        }

        public virtual T As<T>()
            where T : Sprite
        {
            return (T)this;
        }

        public virtual Sprite WithDrawOn(Action<GameTime, SpriteBatch> drawOn)
        {
            DrawOn = drawOn;
            return this;
        }

        public virtual Sprite WithX(int value)
        {
            Position = new Vector2(value, Position.Y);
            return this;
        }

        public virtual Sprite WithY(int value)
        {
            Position = new Vector2(Position.X, value);
            return this;
        }

        public virtual Sprite WithX(float value)
        {
            Position = new Vector2(value, Position.Y);
            return this;
        }

        public virtual Sprite WithY(float value)
        {
            Position = new Vector2(Position.X, value);
            return this;
        }

        #endregion
    }
}