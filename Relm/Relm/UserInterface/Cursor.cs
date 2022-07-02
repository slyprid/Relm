﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Animations;
using Relm.Naming;
using Relm.Sprites;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class Cursor
        : Sprite, IControl
    {
        private Sprite _sprite;
        private Tweener _tweener;
        
        public List<Vector2> Positions { get; set; }
        public int PositionIndex { get; set; }

        public Cursor(TextureAtlas skin)
        {
            TextureAtlas = skin;
            Positions = new List<Vector2>();

            Initialize();
        }

        private void Initialize()
        {
            var w = UserInterfaceSkin.FrameRegionWidth;
            var h = UserInterfaceSkin.FrameRegionHeight;

            Children.Clear();

            _sprite = AddChild<Sprite>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.Cursor))
                .WithPosition(0, 0);

            _tweener = new Tweener();
        }

        private void InitializeTweener()
        {
            _tweener.TweenTo(target: this,
                    expression: x => x.Position,
                    toValue: new Vector2(16, 0),
                    duration: 0.5f,
                    delay: 0.1f)
                .RepeatForever(repeatDelay: 0.2f)
                .AutoReverse()
                .Easing(EasingFunctions.SineInOut);
        }

        public override void Update(GameTime gameTime)
        {
            _tweener.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Positions.Count == 0) return;
            var position = Positions[PositionIndex];
            _sprite.Position = position;
            _sprite.Draw(gameTime, spriteBatch);
        }

        public void Next()
        {
            PositionIndex++;
            if (PositionIndex > Positions.Count - 1)
            {
                PositionIndex = 0;
            }
        }

        public void Previous()
        {
            PositionIndex--;
            if (PositionIndex < 0)
            {
                PositionIndex = Positions.Count - 1;
            }
        }

        public override string ToString()
        {
            return $"{Position} => {_sprite.Position}";
        }

        #region Fluent Functions

        public Cursor RegisterPosition(int x, int y)
        {
            Positions.Add(new Vector2(x, y));
            InitializeTweener();
            return this;
        }

        #endregion
    }
}