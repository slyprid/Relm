using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;

namespace Relm.Sprites
{
    public class AnimatedSprite
        : Sprite
    {
        private double _elapsed;
        private int _frameDirection;
        private int _maxFrame;

        public TextureAtlas Atlas { get; set; }
        public override Texture2D Texture => Atlas.Texture;
        public int FrameWidth { get; set; }
        public int FrameHeight { get; set; }
        public int FrameDuration { get; set; }
        public int CurrentFrame { get; private set; }
        public bool IsLooping { get; set; }
        public bool ReverseOnLoop { get; set; }
        public Action<AnimatedSprite> OnComplete { get; set; }
        public bool IsComplete { get; set; }
        
        public AnimatedSprite(Texture2D texture, int frameWidth, int frameHeight, int frameDuration)
        {
            var name = Guid.NewGuid().ToString();
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameDuration = frameDuration;
            CurrentFrame = 0;
            IsLooping = false;
            ReverseOnLoop = false;
            IsComplete = false;
            _frameDirection = 1;
            var regions = new Dictionary<string, Rectangle>();

            var idx = 0;
            for (var y = 0; y < texture.Height; y += frameHeight)
            {
                for (var x = 0; x < texture.Width; x += frameWidth)
                {
                    regions.Add($"frame{idx}", new Rectangle(x, y, frameWidth, frameHeight));
                    idx++;
                }
            }

            _maxFrame = idx - 1;

            Atlas = new TextureAtlas(name, texture, regions);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsEnabled || IsComplete) return;

            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsed >= FrameDuration)
            {
                CurrentFrame += _frameDirection;

                if (CurrentFrame > _maxFrame || CurrentFrame < 0)
                {
                    OnComplete?.Invoke(this);

                    if (IsLooping && ReverseOnLoop)
                    {
                        _frameDirection *= -1;
                        CurrentFrame += (_frameDirection * 2);
                    }
                    else if (IsLooping && !ReverseOnLoop)
                    {
                        CurrentFrame = 0;
                    }
                    else
                    {
                        IsComplete = true;
                        CurrentFrame -= _frameDirection;
                    }
                }
                _elapsed = 0;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var frame = Atlas[$"frame{CurrentFrame}"];
            spriteBatch.Draw(frame, Position, Color.White.WithOpacity(Opacity));
        }
    }
}