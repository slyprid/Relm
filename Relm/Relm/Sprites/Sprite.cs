using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Entities;
using Relm.Extensions;
using Relm.States;

namespace Relm.Sprites
{
    public class Sprite
        : Entity
    {
        private string _textureName;

        public override string Name { get; protected set; }

        public string TextureName
        {
            get => _textureName;
            set
            {
                _textureName = value;
                Size = new Point(Texture.Width, Texture.Height);
            }
        }

        public Texture2D Texture => GameState.Textures[_textureName];

        public virtual Rectangle Bounds => new Rectangle(X, Y, (int)(Width * Scale), (int)(Height * Scale));

        public Sprite()
        {
            Name = Guid.NewGuid().ToString();
        }

        public Sprite(string name)
        {
            Name = name;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(Width * Scale), (int)(Height * Scale)), new Rectangle(0, 0, Width, Height), Tint.WithOpacity(Opacity)); 
        }

        #region Fluent Functions

        public Sprite IsCentered(CenteringDirection direction)
        {
            if (string.IsNullOrEmpty(TextureName)) return this;

            var x = Position.X;
            var y = Position.Y;
            switch (direction)
            {
                case CenteringDirection.Both:
                    x = (PositionConstants.CenterScreen.X) - (Width / 2);
                    y = (PositionConstants.CenterScreen.Y) - (Height / 2);
                    break;
                case CenteringDirection.Horizontal:
                    x = (PositionConstants.CenterScreen.X) - (Width / 2);
                    break;
                case CenteringDirection.Vertical:
                    y = (PositionConstants.CenterScreen.Y) - (Height / 2);
                    break;
            }

            Position = new Point(x, y);

            return this;
        }

        public Sprite OffsetX(int x)
        {
            Position += new Point(x, 0);
            return this;
        }

        public Sprite OffsetY(int y)
        {
            Position += new Point(0, y);
            return this;
        }

        #endregion
    }
}