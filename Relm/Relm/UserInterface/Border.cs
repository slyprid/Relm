﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Content;
using Relm.Extensions;
using Relm.Naming;
using Relm.Sprites;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class Border
        : Sprite, IControl
    {
        public Color StartBackgroundColor { get; set; }
        public Color EndBackgroundColor { get; set; }

        public Border(TextureAtlas skin)
        {
            TextureAtlas = skin;
            Width = 96;
            Height = 96;
            StartBackgroundColor = Color.Transparent;
            EndBackgroundColor = Color.Transparent;

            Initialize();
        }

        private void Initialize()
        {
            var w = UserInterfaceSkin.FrameRegionWidth;
            var h = UserInterfaceSkin.FrameRegionHeight;

            Children.Clear();

            // Top Left
            AddChild<Sprite>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameTopLeft))
                .WithPosition(0, 0);

            int xo;
            for (xo = 0; xo <= Width - (w * 2); xo += w)
            {
                // Top
                AddChild<Sprite>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameTop))
                    .WithPosition(w + xo, 0);
            }

            // Top Right
            AddChild<Sprite>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameTopRight))
                .WithPosition(w + xo, 0);

            int yo;
            for (yo = 0; yo <= Height - (h * 2); yo += h)
            {
                // Left
                AddChild<Sprite>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameLeft))
                    .WithPosition(0, h + yo);

                // Right
                AddChild<Sprite>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameRight))
                    .WithPosition(w + xo, h + yo);
            }

            // Bottom Left
            AddChild<Sprite>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameBottomLeft))
                .WithPosition(0, h + yo);

            for (xo = 0; xo <= Width - (w * 2); xo += w)
            {
                // Bottom 
                AddChild<Sprite>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameBottom))
                    .WithPosition(w + xo, h + yo);
            }

            // Bottom Right
            AddChild<Sprite>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.FrameBottomRight))
                .WithPosition(w + xo, h + yo);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var whitePixel = spriteBatch.GetWhitePixel();

            var width = Width + UserInterfaceSkin.BorderOffsetX;
            var height = Height + UserInterfaceSkin.BorderOffsetY;
            for (var y = 0; y < height; y++)
            {
                var p = ((float)y / (float)height);
                var color = Color.Lerp(StartBackgroundColor, EndBackgroundColor, p);
                spriteBatch.Draw(whitePixel, new Rectangle((int)Position.X + UserInterfaceSkin.BorderOffsetX, (int)Position.Y + UserInterfaceSkin.BorderOffsetY + y, width, 1), color);
            }

            base.Draw(gameTime, spriteBatch);
        }

        #region Fluent Functions

        public new Border WithPosition(Vector2 position)
        {
            Position = position;
            Children.ForEach(x => x.ParentPosition = Position);
            return this;
        }

        public new Border WithPosition(int x, int y)
        {
            return WithPosition(new Vector2(x, y));
        }

        public new Border WithPosition(float x, float y)
        {
            return WithPosition(new Vector2(x, y));
        }

        public Border WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            Children.ForEach(x => x.ParentPosition = Position);
            return this;
        }

        public Border WithSize(int w, int h)
        {
            return WithSize(new Vector2(w, h));
        }

        public Border WithSize(float w, float h)
        {
            return WithSize(new Vector2(w, h));
        }

        public Border WithBackgroundColor(Color color)
        {
            StartBackgroundColor = color;
            EndBackgroundColor = color;
            return this;
        }

        public Border WithBackgroundColor(Color startColor, Color endColor)
        {
            StartBackgroundColor = startColor;
            EndBackgroundColor = endColor;
            return this;
        }

        #endregion
    }
}