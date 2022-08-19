using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Relm.Collections;
using Relm.Entities;
using Relm.Graphics;

namespace Relm.Components
{
	public class ComponentList
	{
		static IUpdateableComparer compareUpdatableOrder = new IUpdateableComparer();

		private readonly Entity _entity;
        private bool _isComponentListUnsorted;

        private FastList<Component> _components = new();
        private FastList<IUpdateable> _updatableComponents = new();
        private List<Component> _componentsToRemove = new();
        private List<Component> _tempBufferList = new();

		internal List<Component> _componentsToAdd = new();
        public int Count => _components.Length;
        public Component this[int index] => _components.Buffer[index];

		public ComponentList(Entity entity)
		{
			_entity = entity;
		}
		
		public void MarkEntityListUnsorted()
		{
			_isComponentListUnsorted = true;
		}

		public void Add(Component component)
		{
			_componentsToAdd.Add(component);
		}

		public void Remove(Component component)
		{
			Debug.WarnIf(_componentsToRemove.Contains(component), "You are trying to remove a Component ({0}) that you already removed", component);

			if (_componentsToAdd.Contains(component))
			{
				_componentsToAdd.Remove(component);
				return;
			}

			_componentsToRemove.Add(component);
		}

		public void RemoveAllComponents()
		{
			for (var i = 0; i < _components.Length; i++) HandleRemove(_components.Buffer[i]);

			_components.Clear();
			_updatableComponents.Clear();
			_componentsToAdd.Clear();
			_componentsToRemove.Clear();
		}

		internal void DeregisterAllComponents()
		{
			for (var i = 0; i < _components.Length; i++)
			{
				var component = _components.Buffer[i];

				if (component is RenderableComponent) _entity.Scene.RenderableComponents.Remove(component as RenderableComponent);

				if (component is IUpdateable) _updatableComponents.Remove(component as IUpdateable);
			}
		}

		internal void RegisterAllComponents()
		{
			for (var i = 0; i < _components.Length; i++)
			{
				var component = _components.Buffer[i];
				if (component is RenderableComponent) _entity.Scene.RenderableComponents.Add(component as RenderableComponent);

				if (component is IUpdateable) _updatableComponents.Add(component as IUpdateable);
			}
		}

		void UpdateLists()
		{
			if (_componentsToRemove.Count > 0)
			{
				for (int i = 0; i < _componentsToRemove.Count; i++)
				{
					HandleRemove(_componentsToRemove[i]);
					_components.Remove(_componentsToRemove[i]);
				}

				_componentsToRemove.Clear();
			}

			if (_componentsToAdd.Count > 0)
			{
				for (int i = 0, count = _componentsToAdd.Count; i < count; i++)
				{
					var component = _componentsToAdd[i];
					if (component is RenderableComponent) _entity.Scene.RenderableComponents.Add(component as RenderableComponent);

					if (component is IUpdateable) _updatableComponents.Add(component as IUpdateable);

					_components.Add(component);
					_tempBufferList.Add(component);
				}

				_componentsToAdd.Clear();
				_isComponentListUnsorted = true;

				for (var i = 0; i < _tempBufferList.Count; i++)
				{
					var component = _tempBufferList[i];
					component.OnAddedToEntity();

					if (component.Enabled) component.OnEnabled();
				}

				_tempBufferList.Clear();
			}

			if (_isComponentListUnsorted)
			{
				_updatableComponents.Sort(compareUpdatableOrder);
				_isComponentListUnsorted = false;
			}
		}

		void HandleRemove(Component component)
		{
			if (component is RenderableComponent) _entity.Scene.RenderableComponents.Remove(component as RenderableComponent);

			if (component is IUpdateable) _updatableComponents.Remove(component as IUpdateable);

			component.OnRemovedFromEntity();
			component.Entity = null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T GetComponent<T>(bool onlyReturnInitializedComponents) where T : Component
		{
			for (var i = 0; i < _components.Length; i++)
			{
				var component = _components.Buffer[i];
				if (component is T)
					return component as T;
			}

			if (!onlyReturnInitializedComponents)
			{
				for (var i = 0; i < _componentsToAdd.Count; i++)
				{
					var component = _componentsToAdd[i];
					if (component is T)
						return component as T;
				}
			}

			return null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void GetComponents<T>(List<T> components) where T : class
		{
			for (var i = 0; i < _components.Length; i++)
			{
				var component = _components.Buffer[i];
				if (component is T)
					components.Add(component as T);
			}

			for (var i = 0; i < _componentsToAdd.Count; i++)
			{
				var component = _componentsToAdd[i];
				if (component is T)
					components.Add(component as T);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public List<T> GetComponents<T>() where T : class
		{
			var components = ListPool<T>.Obtain();
			GetComponents(components);

			return components;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void Update()
		{
			UpdateLists();
			for (var i = 0; i < _updatableComponents.Length; i++)
			{
				if (_updatableComponents.Buffer[i].Enabled && (_updatableComponents.Buffer[i] as Component).Enabled)
					_updatableComponents.Buffer[i].Update();
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal void OnEntityTransformChanged(Transform.Component comp)
		{
			for (var i = 0; i < _components.Length; i++)
			{
				if (_components.Buffer[i].Enabled)
					_components.Buffer[i].OnEntityTransformChanged(comp);
			}

			for (var i = 0; i < _componentsToAdd.Count; i++)
			{
				if (_componentsToAdd[i].Enabled)
					_componentsToAdd[i].OnEntityTransformChanged(comp);
			}
		}

		internal void OnEntityEnabled()
		{
			for (var i = 0; i < _components.Length; i++)
				_components.Buffer[i].OnEnabled();
		}

		internal void OnEntityDisabled()
		{
			for (var i = 0; i < _components.Length; i++)
				_components.Buffer[i].OnDisabled();
		}

		internal void DebugRender(SpriteBatch spriteBatch)
		{
			for (var i = 0; i < _components.Length; i++)
			{
				if (_components.Buffer[i].Enabled)
					_components.Buffer[i].DebugRender(spriteBatch);
			}
		}
	}
}