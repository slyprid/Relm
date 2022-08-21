using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Components;
using Relm.Graphics;
using Relm.Graphics.Tweening;
using Relm.Graphics.Tweening.Interfaces;
using Relm.Math;
using Relm.Scenes;

namespace Relm.Entities
{
    public class Entity 
        : IComparable<Entity>
	{
		static uint _idGenerator;

        private int _tag = 0;
        private bool _enabled = true;
        internal int _updateOrder = 0;
        internal bool _isDestroyed;

		public Scene Scene { get; set; }
        public string Name;
        public uint Id { get; }
        public Transform Transform { get; }
        public ComponentList Components { get; }
        public uint UpdateInterval = 1;
        public bool IsDestroyed => _isDestroyed;

		public int Tag
		{
			get => _tag;
			set => SetTag(value);
		}
		
		public bool Enabled
		{
			get => _enabled;
			set => SetEnabled(value);
		}

		public int UpdateOrder
		{
			get => _updateOrder;
			set => SetUpdateOrder(value);
		}
		public Entity(string name)
        {
            Components = new ComponentList(this);
            Transform = new Transform(this);
            Name = name;
            Id = _idGenerator++;
        }

        public Entity() : this(Utils.RandomString(8)) { }


		public Transform Parent
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.Parent;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetParent(value);
		}

		public int ChildCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.ChildCount;
		}

		public Vector2 Position
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.Position;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetPosition(value);
		}

		public Vector2 LocalPosition
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.LocalPosition;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetLocalPosition(value);
		}

		public float Rotation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.Rotation;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetRotation(value);
		}

		public float RotationDegrees
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.RotationDegrees;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetRotationDegrees(value);
		}

		public float LocalRotation
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.LocalRotation;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetLocalRotation(value);
		}

		public float LocalRotationDegrees
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.LocalRotationDegrees;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetLocalRotationDegrees(value);
		}

		public Vector2 Scale
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.Scale;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetScale(value);
		}

		public Vector2 LocalScale
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.LocalScale;
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			set => Transform.SetLocalScale(value);
		}

		public Matrix2D WorldInverseTransform
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.WorldInverseTransform;
		}

		public Matrix2D LocalToWorldTransform
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.LocalToWorldTransform;
		}

		public Matrix2D WorldToLocalTransform
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => Transform.WorldToLocalTransform;
		}
		
		internal void OnTransformChanged(Transform.Component comp)
		{
			Components.OnEntityTransformChanged(comp);
		}
		
		public Entity SetTag(int tag)
		{
			if (_tag != tag)
			{
				if (Scene != null) Scene.Entities.RemoveFromTagList(this);
				_tag = tag;
				if (Scene != null) Scene.Entities.AddToTagList(this);
			}

			return this;
		}

		public Entity SetEnabled(bool isEnabled)
		{
			if (_enabled != isEnabled)
			{
				_enabled = isEnabled;

				if (_enabled) Components.OnEntityEnabled();
				else Components.OnEntityDisabled();
			}

			return this;
		}

		public Entity SetUpdateOrder(int updateOrder)
		{
			if (_updateOrder != updateOrder)
			{
				_updateOrder = updateOrder;
				if (Scene != null)
				{
					Scene.Entities.MarkEntityListUnsorted();
					Scene.Entities.MarkTagUnsorted(Tag);
				}
			}

			return this;
		}
		
		public void Destroy()
		{
			_isDestroyed = true;
			Scene.Entities.Remove(this);
			Transform.Parent = null;

			for (var i = Transform.ChildCount - 1; i >= 0; i--)
			{
				var child = Transform.GetChild(i);
				child.Entity.Destroy();
			}
		}

		public void DetachFromScene()
		{
			Scene.Entities.Remove(this);
			Components.DeregisterAllComponents();

			for (var i = 0; i < Transform.ChildCount; i++)
				Transform.GetChild(i).Entity.DetachFromScene();
		}

		public void AttachToScene(Scene newScene)
		{
			Scene = newScene;
			newScene.Entities.Add(this);
			Components.RegisterAllComponents();

			for (var i = 0; i < Transform.ChildCount; i++)
				Transform.GetChild(i).Entity.AttachToScene(newScene);
		}

		public virtual Entity Clone(Vector2 position = default(Vector2))
		{
			var entity = Activator.CreateInstance(GetType()) as Entity;
			entity.Name = Name + "(clone)";
			entity.CopyFrom(this);
			entity.Transform.Position = position;

			return entity;
		}

		protected void CopyFrom(Entity entity)
		{
			Tag = entity.Tag;
			UpdateInterval = entity.UpdateInterval;
			UpdateOrder = entity.UpdateOrder;
			Enabled = entity.Enabled;

			Transform.Scale = entity.Transform.Scale;
			Transform.Rotation = entity.Transform.Rotation;

			for (var i = 0; i < entity.Components.Count; i++)
				AddComponent(entity.Components[i].Clone());
			for (var i = 0; i < entity.Components._componentsToAdd.Count; i++)
				AddComponent(entity.Components._componentsToAdd[i].Clone());

			for (var i = 0; i < entity.Transform.ChildCount; i++)
			{
				var child = entity.Transform.GetChild(i).Entity;

				var childClone = child.Clone();
				childClone.Transform.CopyFrom(child.Transform);
				childClone.Transform.Parent = Transform;
			}
		}


		public virtual void OnAddedToScene() { }

		public virtual void OnRemovedFromScene()
		{
			if (_isDestroyed) Components.RemoveAllComponents();
		}

		public virtual void Update() => Components.Update();

		public virtual void DebugRender(SpriteBatch spriteBatch) => Components.DebugRender(spriteBatch);
		
		public T AddComponent<T>(T component) where T : Component
		{
			component.Entity = this;
			Components.Add(component);
			component.Initialize();
			return component;
		}
		
		public T AddComponent<T>() where T : Component, new()
		{
			var component = new T();
			component.Entity = this;
			Components.Add(component);
			component.Initialize();
			return component;
		}

		public T GetComponent<T>() where T : Component => Components.GetComponent<T>(false);

		public bool TryGetComponent<T>(out T component) where T : Component
		{
			component = Components.GetComponent<T>(false);
			return component != null;
		}

		public bool HasComponent<T>() where T : Component => Components.GetComponent<T>(false) != null;

		public T GetOrCreateComponent<T>() where T : Component, new()
		{
			var comp = Components.GetComponent<T>(true);
			if (comp == null)
				comp = AddComponent<T>();

			return comp;
		}

		public T GetComponent<T>(bool onlyReturnInitializedComponents) where T : Component
		{
			return Components.GetComponent<T>(onlyReturnInitializedComponents);
		}

		public void GetComponents<T>(List<T> componentList) where T : class => Components.GetComponents(componentList);

		public List<T> GetComponents<T>() where T : Component => Components.GetComponents<T>();

		public bool RemoveComponent<T>() where T : Component
		{
			var comp = GetComponent<T>();
			if (comp != null)
			{
				RemoveComponent(comp);
				return true;
			}

			return false;
		}

		public void RemoveComponent(Component component) => Components.Remove(component);

		public void RemoveAllComponents()
		{
			for (var i = 0; i < Components.Count; i++)
				RemoveComponent(Components[i]);
		}
		
		public int CompareTo(Entity other)
		{
			var compare = _updateOrder.CompareTo(other._updateOrder);
			if (compare == 0)
				compare = Id.CompareTo(other.Id);
			return compare;
		}

		public override string ToString()
		{
			return $"[Entity: name: {Name}, tag: {Tag}, enabled: {Enabled}, depth: {UpdateOrder}]";
		}
	}
}