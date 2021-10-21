using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class HeaderPanel
        : Container
    {
        public TextureAtlas TextureAtlas { get; set; }
        public string Header { get; set; }
        public Color HeaderColor { get; set; }
        public int FontSize { get; set; }
        public bool HasTextShadow { get; set; }
        public Vector2 TextShadowOffset { get; set; }
        public int HeaderHeight { get; set; }
        public Vector2 HeaderOffset { get; set; }

        public HeaderPanel()
        {
            HeaderColor = Color.Black;
            FontSize = 16;
            HasTextShadow = true;
            TextShadowOffset = new Vector2(2f, 2f);
            HeaderOffset = Vector2.Zero;
        }

        public override void Configure()
        {
            var config = (HeaderPanelConfig)UserInterface.Skin.ControlConfigurations[typeof(HeaderPanel)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Pieces);
            Size = new Vector2(config.Width, config.Height);
            HeaderHeight = config.HeaderHeight;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Top Left
            var piece = TextureAtlas[PanelPiece.TopLeft.ToString()];
            var bounds = new Rectangle(X, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));

            // Top Right
            piece = TextureAtlas[PanelPiece.TopRight.ToString()];
            bounds = new Rectangle(X + Width - piece.Width, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));

            // Bottom Left
            piece = TextureAtlas[PanelPiece.BottomLeft.ToString()];
            bounds = new Rectangle(X, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));

            // Bottom Right
            piece = TextureAtlas[PanelPiece.BottomRight.ToString()];
            bounds = new Rectangle(X + Width - piece.Width, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));


            // Top Center
            piece = TextureAtlas[PanelPiece.Top.ToString()];
            var startX = X + piece.Width;
            var endX = X + Width - piece.Width;
            for (var x = startX; x < endX; x += piece.Width)
            {
                bounds = new Rectangle(x, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));
            }

            // Bottom Center
            piece = TextureAtlas[PanelPiece.Bottom.ToString()];
            startX = X + piece.Width;
            endX = X + Width - piece.Width;
            for (var x = startX; x < endX; x += piece.Width)
            {
                bounds = new Rectangle(x, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));
            }

            // Left Center
            piece = TextureAtlas[PanelPiece.Left.ToString()];
            var startY = Y + piece.Height;
            var endY = Y + Height - piece.Height;
            for (var y = startY; y < endY; y += piece.Height)
            {
                bounds = new Rectangle(X, y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));
            }

            // Right Center
            piece = TextureAtlas[PanelPiece.Right.ToString()];
            startY = Y + piece.Height;
            endY = Y + Height - piece.Height;
            for (var y = startY; y < endY; y += piece.Height)
            {
                bounds = new Rectangle(X + Width - piece.Width, y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));
            }

            // Center
            piece = TextureAtlas[PanelPiece.Center.ToString()];
            bounds = new Rectangle(X + piece.Width, Y + piece.Width, (int)((Width - (piece.Width * 2)) * Scale.X), (int)((Height - (piece.Height * 2)) * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White.WithOpacity(Opacity));

            if (!string.IsNullOrEmpty(Header))
            {
                var fontSet = UserInterface.Skin.FontSet;
                var font = fontSet[FontSize];
                var textSize = font.MeasureString(Header);
                var textPosition = Position + new Vector2((Width / 2) - (textSize.X / 2), (HeaderHeight / 2) - (textSize.Y / 2));
                if (HasTextShadow)
                {
                    spriteBatch.DrawString(font, Header, textPosition + TextShadowOffset + HeaderOffset, Color.Black.WithOpacity(Opacity));
                }
                spriteBatch.DrawString(font, Header, textPosition + HeaderOffset, HeaderColor.WithOpacity(Opacity));
            }

            base.Draw(gameTime, spriteBatch);
        }

        #region Fluent Functions

        public HeaderPanel SetHeader(string text)
        {
            Header = text;
            return this;
        }

        public HeaderPanel SetHeader(string text, Color color)
        {
            Header = text;
            HeaderColor = color;
            return this;
        }

        public HeaderPanel SetHeader(string text, int fontSize, Color color)
        {
            Header = text;
            HeaderColor = color;
            FontSize = fontSize;
            return this;
        }

        public HeaderPanel SetHeaderOffset(int x, int y)
        {
            HeaderOffset = new Vector2(x, y);
            return this;
        }

        #endregion
    }
}