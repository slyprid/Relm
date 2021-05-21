using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Events;
using Relm.Extensions;
using Relm.Input;
using Relm.Sprites;

namespace Relm.UI.Controls
{
    public class IconButton
        : Button
    {
        public string IconName { get; set; }
        public string IconSheetName { get; set; }

        public SpriteSheet IconSheet => Relm.States.GameState.SpriteSheets[IconSheetName];
        public Rectangle IconSourceRectangle => IconSheet[IconName];
        public float IconScale { get; set; }

        public override Rectangle Bounds
        {
            get
            {
                var part = Skin[IsHover ? PartNames.IconButtonHover : PartNames.IconButton];
                
                var width = (int)(part.Width * Scale);
                var height = (int)(part.Height * Scale);
                return new Rectangle(X, Y, width, height);
            }
        }

        public IconButton()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
            ForegroundColor = Color.Black;
            IconScale = 1f;
        }

        public IconButton(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
            ForegroundColor = Color.Black;
            IconScale = 1f;
        }

        public override void InitializeEvents()
        {
            Scene.AddEvent(new InputEvent($"{Name}:IconButtonMouseOver")
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

            Scene.AddEvent(new InputEvent($"{Name}:IconButtonMouseClick")
            {
                AttachedObject = this,
                InputCheck = (input) => IsHover && input.Mouse.IsPressed(MouseButtons.Left, Bounds),
                OnActivate = (evt, obj) =>
                {
                    OnClick?.Invoke(this);
                }
            });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;

            var part = Skin[IsHover ? PartNames.IconButtonHover : PartNames.IconButton];

            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(part.Width * Scale), (int)(part.Height * Scale)), part, Tint.WithOpacity(Opacity));

            var x = (int)(X + ((Width / 2) - ((IconSheet.Width * Scale) * IconScale) / 2));
            var y = (int)(Y + ((Height / 2) - ((IconSheet.Height * Scale) * IconScale) / 2));
            var w = (int) ((IconSheet.Width * Scale) * IconScale);
            var h = (int) ((IconSheet.Height * Scale) * IconScale);
            spriteBatch.Draw(IconSheet.Texture, new Rectangle(x, y, w, h), IconSourceRectangle, ForegroundColor);
        }
    }
}