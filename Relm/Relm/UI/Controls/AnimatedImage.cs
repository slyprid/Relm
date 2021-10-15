using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.TextureAtlases;

namespace Relm.UI.Controls
{
    public class AnimatedImage
        : Control
    {
        private SpriteSheet _spriteSheet;
        private AnimatedSprite _sprite;

        private TextureAtlas _textureAtlas;

        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public string Key { get; set; }
        public Action<AnimatedImage> OnComplete { get; set; }
        public bool IsPaused { get; set; }

        public AnimatedImage()
        {
            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            if (_sprite == null) CreateSprite();

            var deltaSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _sprite.Play(Key, () =>
            {
                OnComplete?.Invoke(this);
            });
            if (!IsPaused) _sprite.Update(deltaSeconds);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_sprite == null) CreateSprite();

            spriteBatch.Draw(_sprite, Position, 0f, Scale);
        }

        private void CreateSprite()
        {
            _sprite = new AnimatedSprite(_spriteSheet);
        }

        #region Fluent Methods

        public AnimatedImage Using(string textureName)
        {
            Texture = ContentLibrary.Textures[textureName];
            return this;
        }

        public AnimatedImage WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public SpriteSheetAnimationCycle AddCycle(string cycleName, int frameWidth, int frameHeight, float frameDuration = 0.2f, bool isLooping = true)
        {
            Key = cycleName;

            _textureAtlas = TextureAtlas.Create(Guid.NewGuid().ToString(), Texture, frameWidth, frameHeight);
            _spriteSheet = new SpriteSheet { TextureAtlas = _textureAtlas };
            
            var cycle = new SpriteSheetAnimationCycle
            {
                IsLooping = isLooping,
                FrameDuration = frameDuration
            };
            
            _spriteSheet.Cycles.Add(cycleName, cycle);
            return cycle;
        }

        public AnimatedImage OnAnimationComplete(Action<AnimatedImage> action)
        {
            OnComplete = action;
            return this;
        }

        #endregion
    }
}