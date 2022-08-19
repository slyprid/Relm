using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;

namespace Relm.Components
{
    public class Component
    {
        private bool _enabled = true;

        public Entity Entity { get; set; }

        public Transform Transform => Entity.Transform;

        public bool Enabled
        {
            get => Entity != null ? Entity.Enabled && _enabled : _enabled;
            set => SetEnabled(value);
        }

        public virtual void Initialize() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void OnAddedToEntity() { }
        public virtual void OnRemovedFromEntity() { }
        public virtual void OnEntityTransformChanged(Component component) { }
        public virtual void DebugRender(SpriteBatch spriteBatch) { }
        public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }

        public Component SetEnabled(bool value)
        {
            if (_enabled == value) return this;
            _enabled = value;
            if (_enabled) OnEnabled();
            else OnDisabled();

            return this;
        }

        public override string ToString() => $"[Component: type: {GetType()}]";
    }
}