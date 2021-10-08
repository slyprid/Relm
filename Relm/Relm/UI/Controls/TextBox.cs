using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class TextBox
        : Control
    {
        private bool _caretVisible = true;
        private double _elapsed;

        public TextureAtlas TextureAtlas { get; set; }
        public string Text { get; set; }
        public Color TextColor { get; set; }
        public SpriteFont Font { get; set; }
        public int TextPadding { get; set; }
        public string Caret { get; set; }
        public bool HasFocus { get; set; }
        
        public TextBox()
        {
            TextColor = Color.Black;
            TextPadding = 8;
            Caret = "|";
            HasFocus = false;
        }

        public override void Configure()
        {
            var config = (TextBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(TextBox)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Regions);
            Size = new Vector2(config.Width, config.Height);

            Input.OnKeyTyped(OnKeyTyped, ParentScreen);
        }

        public override void Update(GameTime gameTime)
        {
            if (Font == null) throw new Exception("Font not set on Text Box");

            base.Update(gameTime);

            _elapsed += gameTime.ElapsedGameTime.TotalMilliseconds;

            if (_elapsed >= 500)
            {
                _caretVisible = !_caretVisible;
                _elapsed = 0;
            }

            if (Bounds.Intersects(new Rectangle(MouseState.X, MouseState.Y, 1, 1)))
            {
                Mouse.SetCursor(MouseCursor.IBeam);

                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    HasFocus = true;
                }
            }
            else
            {
                Mouse.SetCursor(MouseCursor.Arrow);

                if (Input.WasMouseJustDown(MouseButton.Left))
                {
                    HasFocus = false;
                }
            }

            if (HasFocus)
            {
                
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Font == null) throw new Exception("Font not set on Text Box");

            var leftPiece = TextureAtlas[(int)TextBoxPiece.Left];
            var bounds = new Rectangle(X, Y, leftPiece.Width, leftPiece.Height);
            spriteBatch.Draw(leftPiece, bounds, Color.White);

            var rightPiece = TextureAtlas[(int)TextBoxPiece.Right];
            bounds = new Rectangle(X + (Width - rightPiece.Width), Y, rightPiece.Width, rightPiece.Height);
            spriteBatch.Draw(rightPiece, bounds, Color.White);

            var centerPiece = TextureAtlas[(int)TextBoxPiece.Center];
            var maxWidth = Width - leftPiece.Width - rightPiece.Width;
            bounds = new Rectangle(X + leftPiece.Width, Y, maxWidth, centerPiece.Height);
            spriteBatch.Draw(centerPiece, bounds, Color.White);

            if (Font != null)
            {
                if (_caretVisible && HasFocus)
                {
                    var text = $"{Text}{Caret}";
                    var txtSize = Font.MeasureString(text);
                    var yOffset = (Height / 2) - (txtSize.Y / 2);
                    if (txtSize.X > maxWidth)
                    {
                        text = text.Replace(Caret, "");
                        while (txtSize.X > maxWidth && text.Length > 0)
                        {
                            text = text.Substring(0, text.Length - 1);
                            txtSize = Font.MeasureString(text);
                        }
                    }
                    spriteBatch.DrawString(Font, text, Position + new Vector2(leftPiece.Width + TextPadding, yOffset), TextColor);
                }
                else if (!string.IsNullOrEmpty(Text))
                {
                    var txtSize = Font.MeasureString(Text);
                    var yOffset = (Height / 2) - (txtSize.Y / 2);
                    if (txtSize.X > maxWidth)
                    {
                        Text = Text.Replace(Caret, "");
                        while (txtSize.X > maxWidth && Text.Length > 0)
                        {
                            Text = Text.Substring(0, Text.Length - 1);
                            txtSize = Font.MeasureString(Text);
                        }
                    }
                    spriteBatch.DrawString(Font, Text, Position + new Vector2(leftPiece.Width + TextPadding, yOffset), TextColor);
                }
            }
        }

        public void OnKeyTyped(object sender, KeyboardEventArgs args)
        {
            if (!HasFocus) return;

            var keyChar = args.Character;
            var key = $"{keyChar}";

            // Backspace
            if (keyChar == 8 && !string.IsNullOrEmpty(Text) && Text.Length > 0)
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
            if (keyChar < 32 && keyChar != 33) return;
            
            if (args.Modifiers == KeyboardModifiers.Shift)
            {
                key = key.ToUpper();
            }

            Text = $"{Text}{key}";
        }

        #region Fluent Methods

        public TextBox SetText(string text)
        {
            Text = text;
            return this;
        }

        public TextBox SetText(string text, Color color)
        {
            Text = text;
            TextColor = color;
            return this;
        }

        public TextBox Using(string fontName)
        {
            Font = ContentLibrary.Fonts[fontName];
            return this;
        }

        #endregion
    }
}