﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using ButtonState = Relm.UI.States.ButtonState;

namespace Relm.UI.Controls
{
    public class Button
        : IControl
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public States.ButtonState State { get; set; }
        public Vector2 Scale { get; set; }
        public Texture2D Icon { get; set; }
        public Vector2 IconSize { get; set; }
        public Vector2 IconOffset { get; set; }


        public int Width => (int)Size.X;
        public int Height => (int) Size.Y;
        public int X => (int)Position.X;
        public int Y => (int) Position.Y;

        public Rectangle Bounds => new Rectangle(X, Y, (int)(Width * Scale.X), (int)(Height * Scale.Y));

        public Button()
        {
            State = ButtonState.Normal;
            Scale = Vector2.One;
        }

        public bool UseExternalInputStates { get; set; }
        public KeyboardStateExtended KeyboardState { get; set; }
        public MouseStateExtended MouseState { get; set; }

        public void Configure<T>(T config) 
            where T : IConfig
        {
            var buttonConfig = (ButtonConfig)(IConfig)config;
            var regions = new Dictionary<string, Rectangle>();
            var w = (int)buttonConfig.SourceSize.X;
            var h = (int)buttonConfig.SourceSize.Y;
            regions.Add(ButtonState.Normal.ToString(), new Rectangle(0, 0, w, h));
            regions.Add(ButtonState.Hover.ToString(), new Rectangle(0, h, w, h));
            regions.Add(ButtonState.Active.ToString(), new Rectangle(0, h * 2, w, h));
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), buttonConfig.Texture, regions);
            Size = new Vector2(w, h);
        }

        public void Update(GameTime gameTime)
        {
            State = ButtonState.Normal;

            if(!UseExternalInputStates) MouseState = MouseExtended.GetState();

            if (!Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1))) return;

            State = ButtonState.Hover;
            if (MouseState.WasButtonJustDown(MouseButton.Left))
            {
                State = ButtonState.Active;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var region = TextureAtlas[(int) State];
            spriteBatch.Draw(region, Bounds, Color.White);

            if (Icon == null) return;
            var iconRect = new Rectangle((int)(X + IconOffset.X), (int)(Y + IconOffset.Y), (int) IconSize.X, (int) IconSize.Y);
            spriteBatch.Draw(Icon, iconRect, Color.White);
        }

        #region Fluent Methods

        public Button SetPosition(int x, int y)
        {
            Position = new Vector2(x, y);
            return this;
        }

        public Button SetScale(float scale)
        {
            Scale = new Vector2(scale, scale);
            return this;
        }

        public Button SetScale(float scaleX, float scaleY)
        {
            Scale = new Vector2(scaleX, scaleY);
            return this;
        }

        public Button HasIcon(Texture2D texture)
        {
            Icon = texture;
            IconSize = new Vector2(texture.Width, texture.Height);
            IconOffset = Vector2.Zero;
            return this;
        }

        public Button HasIcon(Texture2D texture, int iconWidth, int iconHeight)
        {
            Icon = texture;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = Vector2.Zero;
            return this;
        }

        public Button HasIcon(Texture2D texture, int iconWidth, int iconHeight, int offsetX, int offsetY)
        {
            Icon = texture;
            IconSize = new Vector2(iconWidth, iconHeight);
            IconOffset = new Vector2(offsetX, offsetY);
            return this;
        }

        #endregion
    }
}