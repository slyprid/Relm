using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.Extensions;

namespace Relm.UI.Controls
{
    public class Icon
        : Control
    {
        private TextureAtlas _textureAtlas;

        public Texture2D Texture { get; set; }
        public Color Color { get; set; }
        public string Key { get; set; }

        public Icon()
        {
            Color = Color.White;
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_textureAtlas[Key], Bounds, Color.WithOpacity(Opacity));
        }

        #region Fluent Methods

        public Icon Using(string textureName, string key, int width, int height)
        {
            Texture = ContentLibrary.Textures[textureName];
            Size = new Vector2(width, height);
            Key = key;
            _textureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), Texture);
            return this;
        }

        public Icon SizeOf(int srcWidth, int srcHeight)
        {
            Size = new Vector2(srcWidth, srcHeight);
            return this;
        }

        public Icon WithColor(Color color)
        {
            Color = color;
            return this;
        }

        public Icon AddRegion(string regionName, int srcX, int srcY, int width, int height)
        {
            _textureAtlas.CreateRegion(regionName, srcX, srcY, width, height);
            return this;
        }

        #endregion
    }
}