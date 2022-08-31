using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Assets.SpriteAtlases;
using Relm.Graphics.Textures;
using Relm.Math;
using Relm.Extensions;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Controls
{
    public class Border
        : RenderableComponent, IUpdateable
    {
        private SpriteAtlas _atlas;
        private Sprite _topLeft;
        private Sprite _topCenter;
        private Sprite _topRight;
        private Sprite _centerLeft;
        private Sprite _center;
        private Sprite _centerRight;
        private Sprite _bottomLeft;
        private Sprite _bottomCenter;
        private Sprite _bottomRight;
        private int _width;
        private int _height;

        public Color BorderColor { get; set; } = Color.White;
        public Color BackgroundStartColor { get; set; } = Color.Transparent;
        public Color BackgroundEndColor { get; set; } = Color.Transparent;
        
        public override RectangleF Bounds
        {
            get
            {
                if (_areBoundsDirty)
                {
                    _bounds.CalculateBounds(Entity.Transform.Position, _localOffset, Vector2.Zero,
                        Entity.Transform.Scale, Entity.Transform.Rotation, _width,
                        _height);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        public Border(SpriteAtlas atlas, int width, int height)
        {
            _atlas = atlas;

            _topLeft = _atlas.GetSprite("BorderTopLeft");
            _topCenter = _atlas.GetSprite("BorderTopCenter");
            _topRight = _atlas.GetSprite("BorderTopRight");
            _centerLeft = _atlas.GetSprite("BorderCenterLeft");
            _center = _atlas.GetSprite("BorderCenter");
            _centerRight = _atlas.GetSprite("BorderCenterRight");
            _bottomLeft = _atlas.GetSprite("BorderBottomLeft");
            _bottomCenter = _atlas.GetSprite("BorderBottomCenter");
            _bottomRight = _atlas.GetSprite("BorderBottomRight");
            _width = width;
            _height = height;
        }

        public void Update()
        {
            
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            float x, y;
            var pos = Entity.Transform.Position + _localOffset;
            var whitePixel = RelmGraphics.CreateSingleColorTexture(1, 1, Color.White);

            // Draw Background
            for(y = 8; y < _height - 8; y++)
            {
                var p = ((float)y / (float)_height);
                var color = Color.Lerp(BackgroundStartColor, BackgroundEndColor, p);
                spriteBatch.Draw(whitePixel, new Rectangle((int)pos.X, (int)(pos.Y + y), _width, 1), color);
            }

            // Draw Border
            // Top Left
            spriteBatch.Draw(_topLeft, pos, BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);

            // Top Center
            var width = (pos.X + _width - _topRight.SourceRect.Width) - (pos.X + _topLeft.SourceRect.Width);
            for (x = pos.X + _topLeft.SourceRect.Width;
                 x < pos.X + _width - (_topRight.SourceRect.Width);
                 x += _topCenter.SourceRect.Width)
            {
                spriteBatch.Draw(_topCenter, new Vector2(x, pos.Y), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            }

            x = pos.X + _width - (_topRight.SourceRect.Width * 2);
            var w = width - ((int)(width / _topCenter.SourceRect.Width) * _topCenter.SourceRect.Width); 
            var destRect = new Rectangle((int)(x + _topCenter.SourceRect.Width - w), (int)pos.Y, (int)w, _topCenter.SourceRect.Height);
            spriteBatch.Draw(_topCenter.Texture2D, destRect, _topCenter.SourceRect, BorderColor, Entity.Transform.Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth);


            // Top Right
            spriteBatch.Draw(_topRight, pos + new Vector2(_width - _topRight.SourceRect.Width, 0), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);


            // Center Left
            var height = (pos.Y + _height - _bottomLeft.SourceRect.Height) - (pos.Y + _bottomLeft.SourceRect.Height);
            for (y = pos.Y + _topLeft.SourceRect.Height;
                 y < pos.Y + _height - (_bottomLeft.SourceRect.Height);
                 y += _centerLeft.SourceRect.Height)
            {
                spriteBatch.Draw(_centerLeft, new Vector2(pos.X, y), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            }

            y = pos.Y + _height - (_bottomLeft.SourceRect.Height * 2);
            var h = height - ((int)(height / _centerLeft.SourceRect.Height) * _centerLeft.SourceRect.Height);
            destRect = new Rectangle((int)pos.X, (int)(y + _centerLeft.SourceRect.Height - h), (int)_centerLeft.SourceRect.Width, (int)h);
            spriteBatch.Draw(_centerLeft.Texture2D, destRect, _centerLeft.SourceRect, BorderColor, Entity.Transform.Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth);

            // Center Right
            height = (pos.Y + _height - _bottomLeft.SourceRect.Height) - (pos.Y + _bottomLeft.SourceRect.Height);
            for (y = pos.Y + _topLeft.SourceRect.Height;
                 y < pos.Y + _height - (_bottomLeft.SourceRect.Height);
                 y += _centerRight.SourceRect.Height)
            {
                spriteBatch.Draw(_centerRight, new Vector2(pos.X + (_width - (_centerRight.SourceRect.Height)), y), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            }

            y = pos.Y + _height - (_bottomRight.SourceRect.Height * 2);
            h = height - ((int)(height / _centerRight.SourceRect.Height) * _centerRight.SourceRect.Height);
            destRect = new Rectangle((int)(pos.X + (_width - (_centerRight.SourceRect.Height))), (int)(y + _centerRight.SourceRect.Height - h), (int)_centerRight.SourceRect.Width, (int)h);
            spriteBatch.Draw(_centerRight.Texture2D, destRect, _centerRight.SourceRect, BorderColor, Entity.Transform.Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth);

            // Bottom Left
            spriteBatch.Draw(_bottomLeft, pos + new Vector2(0f, (_height - (_bottomCenter.SourceRect.Height))), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);

            // Bottom Center
            width = (pos.X + _width - _bottomRight.SourceRect.Width) - (pos.X + _bottomLeft.SourceRect.Width);
            for (x = pos.X + _bottomLeft.SourceRect.Width;
                 x < pos.X + _width - (_bottomRight.SourceRect.Width);
                 x += _bottomCenter.SourceRect.Width)
            {
                spriteBatch.Draw(_bottomCenter, new Vector2(x, pos.Y + (_height - (_bottomCenter.SourceRect.Height))), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
            }

            x = pos.X + _width - (_bottomRight.SourceRect.Width * 2);
            w = width - ((int)(width / _bottomCenter.SourceRect.Width) * _bottomCenter.SourceRect.Width);
            destRect = new Rectangle((int)(x + _bottomCenter.SourceRect.Width - w), (int)(pos.Y + (_height - (_bottomCenter.SourceRect.Height))), (int)w, _bottomCenter.SourceRect.Height);
            spriteBatch.Draw(_bottomCenter.Texture2D, destRect, _bottomCenter.SourceRect, BorderColor, Entity.Transform.Rotation, Vector2.Zero, SpriteEffects.None, LayerDepth);


            // Bottom Right
            spriteBatch.Draw(_bottomRight, pos + new Vector2(_width - _bottomRight.SourceRect.Width, (_height - (_bottomCenter.SourceRect.Height))), BorderColor, Entity.Transform.Rotation, Vector2.Zero, Entity.Transform.Scale, SpriteEffects.None, LayerDepth);
        }

        public Border SetBorderColor(Color color)
        {
            BorderColor = color;
            return this;
        }

        public Border SetBackgroundColor(Color startColor, Color endColor)
        {
            BackgroundStartColor = startColor;
            BackgroundEndColor = endColor;
            return this;
        }

        public Border SetBackgroundColor(Color color)
        {
            BackgroundStartColor = color;
            BackgroundEndColor = color;
            return this;
        }
    }
}