using System;
using System.Runtime.CompilerServices;
using Relm.Entities;
using Relm.Graphics;

namespace Relm.Components
{
	public class Component 
        : IComparable<Component>
	{
		public Entity Entity;

		public Transform Transform
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Entity.Transform;
		}

		public bool Enabled
		{
			get => Entity != null ? Entity.Enabled && _enabled : _enabled;
			set => SetEnabled(value);
		}

		public int UpdateOrder
		{
			get => _updateOrder;
			set => SetUpdateOrder(value);
		}

		bool _enabled = true;
		internal int _updateOrder = 0;


		public virtual void Initialize() { }
        public virtual void OnAddedToEntity() { }
        public virtual void OnRemovedFromEntity() { }
        public virtual void OnEntityTransformChanged(Transform.Component comp) { }
        public virtual void DebugRender(SpriteBatch spriteBatch) { }
        public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }
		
		public Component SetEnabled(bool isEnabled)
		{
			if (_enabled != isEnabled)
			{
				_enabled = isEnabled;

				if (_enabled)
					OnEnabled();
				else
					OnDisabled();
			}

			return this;
		}

		public Component SetUpdateOrder(int updateOrder)
		{
			if (_updateOrder != updateOrder)
			{
				_updateOrder = updateOrder;
				if (Entity != null) Entity.Components.MarkEntityListUnsorted();
			}

			return this;
		}

		public virtual Component Clone()
		{
			var component = MemberwiseClone() as Component;
			component.Entity = null;

			return component;
		}

		public int CompareTo(Component other) => _updateOrder.CompareTo(other._updateOrder);
        public override string ToString() => $"[Component: type: {GetType()}, updateOrder: {UpdateOrder}]";
	}
}