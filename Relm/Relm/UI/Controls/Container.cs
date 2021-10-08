using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using Relm.Extensions;

namespace Relm.UI.Controls
{
    public class Container
        : Control
    {
        private readonly ControlManager _controls;
        private Vector2 _position;

        public List<IControl> Controls => _controls.Controls;

        public IControl this[string id] => _controls[id];

        public override Vector2 Position
        {
            get => _position;
            set
            {
                foreach (var control in _controls.Controls)
                {
                    control.Position = control.Position - _position;
                    control.Position = value + control.Position;
                }
                _position = value;
            }
        }

        public Container()
        {
            _controls = new ControlManager();
        }

        public override void Update(GameTime gameTime)
        {
            _controls.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _controls.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        #region Fluent Functions

        public T Add<T>(GameScreen parentScreen)
            where T : IControl
        {
            return Add<T>(Guid.NewGuid().ToString(), parentScreen);
        }

        public T Add<T>(string id, GameScreen parentScreen)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var control = _controls.Add<T>(id, parentScreen);
            control.ParentScreen = parentScreen;
            control.Position = Position + control.Position;
            return control;
        }

        public T Add<T>(string id, int x, int y, GameScreen parentScreen)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var control = _controls.Add<T>(id, parentScreen);
            control.ParentScreen = parentScreen;
            control.Position = Position + new Vector2(x, y);
            return control;
        }

        public T Add<T>(string id, Vector2 pos, GameScreen parentScreen)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var control = _controls.Add<T>(id, parentScreen);
            control.ParentScreen = parentScreen;
            control.Position = Position + pos;
            return control;
        }

        public Container Add<T>(GameScreen parentScreen, T control)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var id = Guid.NewGuid().ToString();
            control.ParentScreen = parentScreen;
            control.Position = Position + control.Position;
            _controls.Add(id, parentScreen, control);
            return this;
        }

        public Container Add<T>(string id, GameScreen parentScreen, T control)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            control.ParentScreen = parentScreen;
            control.Position = Position + control.Position;
            _controls.Add(id, parentScreen, control);
            return this;
        }

        #endregion
    }
}