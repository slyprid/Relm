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

        public override string Name { get; }

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

            spriteBatch.Draw(Texture, new Rectangle(X, Y, Width, Height), new Rectangle(0, 0, Width, Height), Tint.WithOpacity(Opacity)); 
        }

        #region Fluent Functions

        public Sprite IsCenterScreen()
        {
            if (string.IsNullOrEmpty(TextureName)) return this;

            Position = new Point((PositionConstants.CenterScreen.X) - (Width / 2), (PositionConstants.CenterScreen.Y) - (Height / 2));

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