using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;
using Relm.Sprites;

namespace Relm.TimeElements
{
    public abstract class TimeElementSprite
        : Sprite
    {
        private readonly int _frameWidth = 48;
        private readonly int _frameHeight = 48;
        private readonly Dictionary<string, List<Rectangle>> _regions;
        private readonly TimeElementContentManager _contentManager;

        private Dictionary<string, string> _parts = new Dictionary<string, string>();

        protected TimeElementSprite(TimeElementContentManager contentManager)
        {
            _contentManager = contentManager;

            _regions = new Dictionary<string, List<Rectangle>>();

            TimeElementAnimationNames.ForEach(animationName =>
            {
                var w = _frameWidth;
                var h = _frameHeight;
                var x = 0;
                var y = 0;

                switch (animationName)
                {
                    case nameof(TimeElementAnimationNames.Walk):
                        x = 0;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),
                                          
                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),
                                          
                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),
                                          
                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.ArmsUp):
                        x = w * 3;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),

                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),

                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),

                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.BowWalk):
                        x = w * 15;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),
                            new Rectangle(x + w * 3, y, w, h),

                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),
                            new Rectangle(x + w * 3, h, w, h),

                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),
                            new Rectangle(x + w * 3, h * 2, w, h),

                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                            new Rectangle(x + w * 3, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.Climb):
                        x = w * 19;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),

                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),

                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),

                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.BowNock):
                        x = w * 15;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x, h, w, h),
                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.Crouch):
                        x = w * 6;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x, h, w, h),
                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.Jump):
                        x = w * 6;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),
                            new Rectangle(x + w * 3, y, w, h),

                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),
                            new Rectangle(x + w * 3, h, w, h),

                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),
                            new Rectangle(x + w * 3, h * 2, w, h),

                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                            new Rectangle(x + w * 3, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.WindUp):
                        x = w * 10;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x, h, w, h),
                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.Attack):
                        x = w * 10;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x + w, y, w, h),
                            new Rectangle(x + w * 2, y, w, h),
                            new Rectangle(x + w * 3, y, w, h),
                            new Rectangle(x + w * 4, y, w, h),

                            new Rectangle(x, h, w, h),
                            new Rectangle(x + w, h, w, h),
                            new Rectangle(x + w * 2, h, w, h),
                            new Rectangle(x + w * 3, h, w, h),
                            new Rectangle(x + w * 4, h, w, h),

                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x + w, h * 2, w, h),
                            new Rectangle(x + w * 2, h * 2, w, h),
                            new Rectangle(x + w * 3, h * 2, w, h),
                            new Rectangle(x + w * 4, h * 2, w, h),

                            new Rectangle(x, h * 3, w, h),
                            new Rectangle(x + w, h * 3, w, h),
                            new Rectangle(x + w * 2, h * 3, w, h),
                            new Rectangle(x + w * 3, h * 3, w, h),
                            new Rectangle(x + w * 4, h * 3, w, h),
                        };
                        break;
                    case nameof(TimeElementAnimationNames.Dead):
                        x = w * 22;
                        y = 0;
                        _regions[animationName] = new List<Rectangle>
                        {
                            new Rectangle(x, y, w, h),
                            new Rectangle(x, h, w, h),
                            new Rectangle(x, h * 2, w, h),
                            new Rectangle(x, h * 3, w, h),
                        };
                        break;
                }
            });

            TimeElementParts.ForEach(partName =>
            {
                var value = $"{partName}_0";
                if (_contentManager.ContainsKey(value))
                {
                    _parts.Add(partName, value);
                }
            });
        }

        public void Randomize()
        {
            _parts.Clear();

            TimeElementParts.ForEach(partName =>
            {
                var value = $"{partName}_{Random.Next(0, _contentManager.MaxIndex(partName))}";
                if (_contentManager.ContainsKey(value))
                {
                    _parts.Add(partName, value);
                }
            });
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var position = CalculatePosition(this);
            var destRect = new Rectangle((int)Position.X, (int)Position.Y, (int)(1104 * Scale.X), (int)(192 * Scale.Y));

            var texture = _contentManager[_parts[TimeElementParts.Shadow]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Bottom]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.BackExtra]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.BackHair]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Head]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Top]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.FrontExtra]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Hair]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Hat]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);

            texture = _contentManager[_parts[TimeElementParts.Weapon]];
            spriteBatch.Draw(texture, destRect, null, Tint.WithOpacity(Opacity), 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}