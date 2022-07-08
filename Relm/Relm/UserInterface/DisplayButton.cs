using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Naming;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class DisplayButton
        : BaseControl
    {
        private readonly UserInterfaceSkin _skin;
        private SpriteFont _font;
        private string _text;

        public string DisplayButtonName { get; set; }
        
        public DisplayButton(UserInterfaceSkin skin) 
            : base(skin)
        {
            _skin = skin;
            DisplayButtonName = nameof(UserInterfaceRegions.None);
        }

        public override void Initialize()
        {
            base.Initialize();

            AddChild<ChildControl>(_skin)
                .WithAtlasRegionName(DisplayButtonName);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            if (string.IsNullOrEmpty(_text)) return;
            var position = Parent.Position + Position + new Vector2(0f, 24f);
            spriteBatch.DrawString(_font, _text, position, Color.Black);
        }

        #region Fluent Functions

        public DisplayButton WithDisplayButton(string value)
        {
            DisplayButtonName = value;
            Initialize();
            return this;
        }

        public DisplayButton WithText(string text)
        {
            _text = text;
            return this;
        }

        public DisplayButton WithFont(SpriteFont font)
        {
            _font = font;
            return this;
        }

        #endregion
    }
}