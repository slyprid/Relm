using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Input;
using Relm.UI.Configuration;
using Relm.UI.Controls;

namespace Relm.UI
{
    public class ControlManager
    {
        private readonly Dictionary<string, IControl> _controls;

        public List<IControl> Controls => _controls.Values.ToList();

        public IControl this[string id] => _controls[id];

        public ControlManager()
        {
            _controls = new Dictionary<string, IControl>();
        }

        public T Get<T>(string id)
        {
            return (T) _controls[id];
        }

        public T Add<T>()
            where T : IControl
        {
            return Add<T>(Guid.NewGuid().ToString());
        }
        
        public T Add<T>(string id)
            where T : IControl
        {
            var control = Activator.CreateInstance<T>();
            control.Configure();
            _controls.Add(id, control);
            return control;
        }

        public T Add<T>(string id, T control)
            where T : IControl
        {
            control.Configure();
            _controls.Add(id, control);
            return control;
        }
        
        public void Remove(string id)
        {
            _controls.Remove(id);
        }

        public void UpdateInput()
        {
            foreach (var control in _controls.Values)
            {
                control.KeyboardState = Input.GetKeyboardState();
                control.MouseState = Input.GetMouseState();
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (var control in _controls.Values)
            {
                control.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var control in _controls.Values)
            {
                control.Draw(gameTime, spriteBatch);
            }
        }
    }
}