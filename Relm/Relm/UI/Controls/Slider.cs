﻿using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Constants;
using Relm.Events;
using Relm.Extensions;
using Relm.Input;
using Relm.States;

namespace Relm.UI.Controls
{
    public class Slider
        : Control
    {
        public override Skin Skin => GameState.UserInterfaceSettings.Skins[typeof(Slider)];

        private bool _isHoverLeft;
        private bool _isHoverRight;

        private Rectangle _partLeft => Skin[_isHoverLeft ? PartNames.SliderLeftButton : PartNames.SliderLeftButtonHover];
        private Rectangle _partRight => Skin[_isHoverRight ? PartNames.SliderRightButton : PartNames.SliderRightButtonHover];

        private Rectangle _partTick
        {
            get
            {
                switch (ColorType)
                {
                    case SliderTypes.Purple:
                        return Skin[PartNames.SliderTick2];
                    case SliderTypes.Yellow:
                        return Skin[PartNames.SliderTick3];
                    case SliderTypes.Red:
                        return Skin[PartNames.SliderTick4];
                    default:
                    case SliderTypes.Green:
                        return Skin[PartNames.SliderTick1];
                }
            }
        }

        private Rectangle _partEmptyTick
        {
            get
            {
                switch (ColorType)
                {
                    case SliderTypes.Purple:
                        return Skin[PartNames.SliderTick2Empty];
                    case SliderTypes.Yellow:
                        return Skin[PartNames.SliderTick3Empty];
                    case SliderTypes.Red:
                        return Skin[PartNames.SliderTick4Empty];
                    default:
                    case SliderTypes.Green:
                        return Skin[PartNames.SliderTick1Empty];
                }
            }
        }

        private Rectangle _leftButtonBounds => new Rectangle(X, Y, (int)(_partLeft.Width * Scale), (int)(_partLeft.Height * Scale));

        private Rectangle _rightButtonBounds
        {
            get
            {
                var x = (int)(X + (_partLeft.Width * Scale) + ((_partTick.Width * Scale) * Ticks)) + (int)((_partTick.Width * Scale) + (2 * Scale));
                return new Rectangle(x, Y, (int)(_partRight.Width * Scale), (int)(_partRight.Height * Scale));
            }
        }

        public int Minimum { get; set; }
        public int Maximum { get; set; }
        public int Value { get; set; }
        public int Ticks { get; set; }
        public int PerTick { get; set; }
        public SliderTypes ColorType { get; set; }

        public Slider()
        {
            Name = Guid.NewGuid().ToString();
            TextureName = TextureNames.UserInterfaceSkin;
            Maximum = 100;
            Value = Maximum;
            Ticks = 10;
            PerTick = 10;
            Scale = 2;
            ColorType = SliderTypes.Green;
        }

        public Slider(string name)
        {
            Name = name;
            TextureName = TextureNames.UserInterfaceSkin;
            Maximum = 100;
            Value = Maximum;
            Ticks = 10;
            PerTick = 10;
            Scale = 2;
            ColorType = SliderTypes.Green;
        }

        public override void InitializeEvents()
        {
            Scene.AddEvent(new InputEvent($"{Name}:LeftButtonMouseOver")
            {
                AttachedObject = this,
                InputCheck = (input) => input.Mouse.Bounds.Intersects(_leftButtonBounds),
                OnActivate = (evt, obj) =>
                {
                    _isHoverLeft = true;
                },
                OnDeactivate = (evt, obj) =>
                {
                    _isHoverLeft = false;
                }
            });

            Scene.AddEvent(new InputEvent($"{Name}:LeftButtonMouseClick")
            {
                AttachedObject = this,
                InputCheck = (input) => _isHoverLeft && input.Mouse.IsHeld(MouseButtons.Left, Bounds),
                OnActivate = (evt, obj) =>
                {
                    Value--;
                }
            });

            Scene.AddEvent(new InputEvent($"{Name}:RightButtonMouseOver")
            {
                AttachedObject = this,
                InputCheck = (input) => input.Mouse.Bounds.Intersects(_rightButtonBounds),
                OnActivate = (evt, obj) =>
                {
                    _isHoverRight = true;
                },
                OnDeactivate = (evt, obj) =>
                {
                    _isHoverRight = false;
                }
            });

            Scene.AddEvent(new InputEvent($"{Name}:RightButtonMouseClick")
            {
                AttachedObject = this,
                InputCheck = (input) => _isHoverRight && input.Mouse.IsHeld(MouseButtons.Left, Bounds),
                OnActivate = (evt, obj) =>
                {
                    Value++;
                }
            });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Value < Minimum) Value = Minimum;
            if (Value > Maximum) Value = Maximum;

            Relm.States.GameState.DebugValue = Value;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!IsVisible) return;
            
            spriteBatch.Draw(Texture, new Rectangle(X, Y, (int)(_partLeft.Width * Scale), (int)(_partLeft.Height * Scale)), _partLeft, Tint.WithOpacity(Opacity));

            var x = 0;
            var fullTicks = Value / PerTick;
            for (var i = 0; i < Ticks; i++)
            {
                x = (int)(X + (_partLeft.Width * Scale) + ((_partTick.Width * Scale) * i));
                var part = fullTicks > i ? _partTick : _partEmptyTick;
                spriteBatch.Draw(Texture, new Rectangle(x, Y, (int)(part.Width * Scale), (int)(part.Height * Scale)), part, Tint.WithOpacity(Opacity));
            }

            x += (int)((_partTick.Width * Scale) + (2 * Scale));
            spriteBatch.Draw(Texture, new Rectangle(x, Y + 1, (int)(_partRight.Width * Scale), (int)(_partRight.Height * Scale)), _partRight, Tint.WithOpacity(Opacity));
        }
    }
}