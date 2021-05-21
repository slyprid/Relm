using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Events;
using Relm.Extensions;
using Relm.Input;
using Relm.States;

namespace Relm.UI.Controls
{
    public class Button
        : Control
    {
        private Point _size;

        public override string Name { get; protected set; }
        public Action<Button> OnClick { get; set; }
        public bool IsHover { get; set; }
        public string Text { get; set; }
        public Color ForegroundColor { get; set; }
        public float FontScale { get; set; }
        public bool IsToggled { get; set; }

        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(Button)];

        public override Point Size
        {
            get
            {
                if (_size == new Point(Texture.Width, Texture.Height))
                {
                    _size = new Point(Bounds.Width, Bounds.Height);
                }
                return _size;
            }
            set => _size = value;
        }

        public override Rectangle Bounds
        {
            get
            {
                var partLeft = Skin[IsHover ? PartNames.ButtonLeftHover : PartNames.ButtonLeft];
                var partCenter = Skin[IsHover ? PartNames.ButtonCenterHover : PartNames.ButtonCenter];
                var partRight = Skin[IsHover ? PartNames.ButtonRightHover : PartNames.ButtonRight];

                var width = (int) (partLeft.Width * Scale) + (int) (partCenter.Width * Scale) + (int) (partRight.Width * Scale);
                var height = (int) (partLeft.Height * Scale);
                return new Rectangle(X, Y, width, height);
            }
        }

        public Button()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
            ForegroundColor = Color.Black;
            FontScale = 1f;
            IsToggled = false;
        }

        public Button(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
            ForegroundColor = Color.Black;
            FontScale = 1f;
            IsToggled = false;
        }

        public override void InitializeEvents()
        {
            Scene.AddEvent(new InputEvent($"{Name}:ButtonMouseOver")
            {
                AttachedObject = this,
                InputCheck = (input) => input.Mouse.Bounds.Intersects(Bounds),
                OnActivate = (evt, obj) =>
                {
                    IsHover = true;
                },
                OnDeactivate = (evt, obj) =>
                {
                    IsHover = false;
                }
            });

            Scene.AddEvent(new InputEvent($"{Name}:ButtonMouseClick")
            {
                AttachedObject = this,
                InputCheck = (input) => IsHover && input.Mouse.IsPressed(MouseButtons.Left, Bounds),
                OnActivate = (evt, obj) =>
                {
                    OnClick?.Invoke(this);
                }
            });
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var partLeft = Skin[IsHover || IsToggled ? PartNames.ButtonLeftHover : PartNames.ButtonLeft];
            var partCenter = Skin[IsHover || IsToggled ? PartNames.ButtonCenterHover : PartNames.ButtonCenter];
            var partRight = Skin[IsHover || IsToggled ? PartNames.ButtonRightHover : PartNames.ButtonRight];

            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(partLeft.Width * Scale), (int)(partLeft.Height * Scale)), partLeft, Tint.WithOpacity(Opacity));
            spriteBatch.Draw(Texture, new Rectangle(X + (int)(partLeft.Width * Scale), Y, (int)(partCenter.Width * Scale), (int)(partCenter.Height * Scale)), partCenter, Tint.WithOpacity(Opacity));
            spriteBatch.Draw(Texture, new Rectangle(X + (int)(partLeft.Width * Scale) + (int)(partCenter.Width * Scale), Y, (int)(partRight.Width * Scale), (int)(partRight.Height * Scale)), partRight, Tint.WithOpacity(Opacity));

            var fontSize = Skin.Font.MeasureString(Text);
            var fontPosition = new Vector2(Position.X + ((Width / 2f) - ((fontSize.X * FontScale) / 2)), Position.Y + ((Height / 2f) - ((fontSize.Y * FontScale) / 2)));
            spriteBatch.DrawString(Skin.Font, Text, fontPosition, ForegroundColor, 0f, Vector2.Zero, FontScale, SpriteEffects.None, 0f);
        }
    }
}