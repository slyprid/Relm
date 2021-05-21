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
    public class CustomButton
        : Control
    {
        private Point _size;

        public override string Name { get; protected set; }
        public Action<CustomButton> OnClick { get; set; }
        public bool IsHover { get; set; }
        public string PartName { get; set; }
        public string HoverPartName { get; set; }
        
        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(CustomButton)];

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
                var part = Skin[IsHover ? HoverPartName : PartName];

                var width = (int)(part.Width * Scale);
                var height = (int)(part.Height * Scale);
                return new Rectangle(X, Y, width, height);
            }
        }

        public CustomButton()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
        }

        public CustomButton(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
        }

        public override void InitializeEvents()
        {
            Scene.AddEvent(new InputEvent($"{Name}:CustomButtonMouseOver")
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

            Scene.AddEvent(new InputEvent($"{Name}:CustomButtonMouseClick")
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

            var part = Skin[IsHover ? HoverPartName : PartName];

            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(part.Width * Scale), (int)(part.Height * Scale)), part, Tint.WithOpacity(Opacity), Rotation, Origin, Effects, 0f);
        }
    }
}
