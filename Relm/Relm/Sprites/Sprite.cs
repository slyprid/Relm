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
        public string SourceName { get; set; }

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

            var srcRect = new Rectangle(0, 0, Width, Height);
            if (!string.IsNullOrEmpty(SourceName))
            {
                srcRect = Relm.States.GameState.SpriteSheets[TextureName][SourceName];
                Size = new Point(srcRect.Width, srcRect.Height);
            }
            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(Width * Scale), (int)(Height * Scale)), srcRect, Tint.WithOpacity(Opacity), Rotation, Origin, Effects, 0f); 
        }

        #region Fluent Functions

        public virtual Sprite IsCentered(CenteringDirection direction)
        {
            if (string.IsNullOrEmpty(TextureName)) return this;

            if (!string.IsNullOrEmpty(SourceName))
            {
                var srcRect = Relm.States.GameState.SpriteSheets[TextureName][SourceName];
                Size = new Point((int)(srcRect.Width * Scale), (int)(srcRect.Height * Scale));
            }

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

        public Sprite Anchored(AnchoringPositions anchoringPositions)
        {
            if (string.IsNullOrEmpty(TextureName)) return this;

            var x = Position.X;
            var y = Position.Y;
            switch (anchoringPositions)
            {

                case AnchoringPositions.TopLeft:
                    x = 0;
                    y = 0;
                    break;
                case AnchoringPositions.TopCenter:
                    x = (PositionConstants.CenterScreen.X) - (Width / 2);
                    y = 0;
                    break;
                case AnchoringPositions.TopRight:
                    x = GameCore.ResolutionWidth - Width;
                    y = 0;
                    break;
                case AnchoringPositions.CenterLeft:
                    x = 0;
                    y = (PositionConstants.CenterScreen.Y) - (Height / 2);
                    break;
                case AnchoringPositions.Center:
                    x = (PositionConstants.CenterScreen.X) - (Width / 2);
                    y = (PositionConstants.CenterScreen.Y) - (Height / 2);
                    break;
                case AnchoringPositions.CenterRight:
                    x = GameCore.ResolutionWidth - Width;
                    y = (PositionConstants.CenterScreen.Y) - (Height / 2);
                    break;
                case AnchoringPositions.BottomLeft:
                    x = 0;
                    y = GameCore.ResolutionHeight - Height;
                    break;
                case AnchoringPositions.BottomCenter:
                    x = (PositionConstants.CenterScreen.X) - (Width / 2);
                    y = GameCore.ResolutionHeight - Height;
                    break;
                case AnchoringPositions.BottomRight:
                    x = GameCore.ResolutionWidth - Width;
                    y = GameCore.ResolutionHeight - Height;
                    break;

                case AnchoringPositions.None:
                default:
                    break;
            }

            Position = new Point(x, y);

            return this;
        }

        #endregion
    }
}