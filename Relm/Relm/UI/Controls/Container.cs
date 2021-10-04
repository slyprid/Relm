using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;

namespace Relm.UI.Controls
{
    public class Container
        : Control
    {
        private readonly ControlManager _controls;
        private Vector2 _position;

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

        public T Add<T>()
            where T : IControl
        {
            return Add<T>(Guid.NewGuid().ToString());
        }

        public T Add<T>(string id)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var control = _controls.Add<T>(id);
            control.Position = Position + control.Position;
            return control;
        }

        public Container Add<T>(T control)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            var id = Guid.NewGuid().ToString();
            control.Position = Position + control.Position;
            _controls.Add(id, control);
            return this;
        }

        public Container Add<T>(string id, T control)
            where T : IControl
        {
            if (Position == Vector2.Zero) throw new Exception("Must set position of container before adding controls");
            control.Position = Position + control.Position;
            _controls.Add(id, control);
            return this;
        }

        #endregion
    }
}