using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class Panel
        : Control
    {
        public TextureAtlas TextureAtlas { get; set; }

        public override void Configure()
        {
            var config = (PanelConfig)UserInterface.Skin.ControlConfigurations[typeof(Panel)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Pieces);
            Size = new Vector2(config.Width, config.Height);
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
                bounds = new Rectangle(x, Y, (int) (piece.Width * Scale.X), (int) (piece.Height * Scale.Y));
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
        }
    }
}