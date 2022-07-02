using System;
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
        public Vector2 Position { get; set; }
        public virtual Vector2 ParentPosition { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Vector2 Size => new Vector2(Width, Height);
        public Texture2D Texture { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public Color Tint { get; set; }
        public string AtlasRegionName { get; set; }
        public List<Sprite> Children { get; set; }
        public Vector2 Scale { get; set; }

        public Sprite()
        {
            Tint = Color.White;
            Children = new List<Sprite>();
            ParentPosition = new Vector2(float.MinValue, float.MinValue);
            Scale = Vector2.One;
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Children.ForEach(x => x.ParentPosition = Position);
            Children.ForEach(x => x.Update(gameTime));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var position = Position;
            if (ParentPosition != new Vector2(float.MinValue, float.MinValue))
            {
                position += ParentPosition;
            }

            if (Texture != null)
            {
                var destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));
                spriteBatch.Draw(Texture, destRect, null, Tint, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }
            else
            {
                // Checks to see if this is a container sprite
                if (!string.IsNullOrEmpty(AtlasRegionName))
                {
                    var region = TextureAtlas.GetRegion(AtlasRegionName);
                    Width = region.Width;
                    Height = region.Height;
                    spriteBatch.Draw(region, position, Tint, 0f, Vector2.Zero, Scale, SpriteEffects.None, 0f, null);
                }
            }

            Children.ForEach(x => x.Draw(gameTime, spriteBatch));
        }

        public virtual T AddChild<T>(params object[] args)
            where T : Sprite
        {
            var child = (T)Activator.CreateInstance(typeof(T), args);
            Children.Add(child);
            return child;
        }

        #region Fluent Functions

        public virtual Sprite WithAtlasRegionName(string value)
        {
            AtlasRegionName = value;
            return this;
        }

        public virtual Sprite WithPosition(Vector2 position)
        {
            Position = position;
            return this;
        }

        public virtual Sprite WithPosition(int x, int y)
        {
            return WithPosition(new Vector2(x, y));
        }

        public virtual Sprite WithPosition(float x, float y)
        {
            return WithPosition(new Vector2(x, y));
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

        public virtual Sprite WithPositionOffset(Vector2 offset)
        {
            Position += offset;
            return this;
        }

        public virtual Sprite WithPositionOffset(int x, int y)
        {
            return WithPositionOffset(new Vector2(x, y));
        }

        public virtual Sprite WithPositionOffset(float x, float y)
        {
            return WithPositionOffset(new Vector2(x, y));
        }

        public virtual Sprite WithScale(Vector2 scale)
        {
            Scale = scale;
            Children.ForEach(x => x.Scale = scale);
            return this;
        }

        public virtual T As<T>()
            where T : Sprite
        {
            return (T)this;
        }

        #endregion
    }
}