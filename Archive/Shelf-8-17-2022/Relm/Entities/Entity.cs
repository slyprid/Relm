using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;
using Relm.Scenes;

namespace Relm.Entities
{
    public class Entity
    {
        private bool _enabled;
        private int _tag;
        
        public int Id { get; set; }
        public Scene Scene { get; set; }
        public string Name { get; set; }
        public Transform Transform { get; }
        public ComponentList Components { get; }
        public bool IsDestroyed { get; set; }
        public uint UpdateInterval { get; set; } = 1;

        public bool Enabled
        {
            get => _enabled;
            set => SetEnabled(value);
        }

        public int Tag
        {
            get => _tag;
            set => SetTag(value);
        }

        public Entity(string name)
        {
            Components = new ComponentList(this);
            Transform = new Transform(this);
            Name = name;
        }

        public Entity() : this(Guid.NewGuid().ToString()) { }

        public Entity SetEnabled(bool isEnabled)
        {
            if (_enabled == isEnabled) return this;

            _enabled = isEnabled;

            if (_enabled) Components.OnEntityEnabled();
            else Components.OnEntityDisabled();

            return this;
        }

        internal void OnEnabled()
        {
            for (var i = 0; i < _components.Length; i++)
                _components.Buffer[i].OnEnabled();
        }

        public Entity SetTag(int tag)
        {
            if (_tag == tag) return this;
            if (Scene != null) Scene.Entities.RemoveFromTagList(this);
            _tag = tag;
            if (Scene != null) Scene.Entities.AddToTagList(this);
            return this;
        }

        internal void OnDisabled()
        {
            for (var i = 0; i < _components.Length; i++)
                _components.Buffer[i].OnDisabled();
        }

        internal void OnTransformChanged(Component component)
        {
            Components.OnEntityTranformChanged(component);
        }

        public void Destroy()
        {
            IsDestroyed = true;
            Scene.Entities.Remove(this);
            Transform.Parent = null;

            for (var i = Transform.ChildCount - 1; i >= 0; i--)
            {
                var child = Transform.GetChild(i);
                child.Entity.Destroy();
            }
        }

        public void DetatchFromScene()
        {
            Scene.Entities.Remove(this);
            Components.DeregisterAllComponents();

            for (var i = 0; i < Transform.ChildCount; i++)
            {
                Transform.GetChild(i).Entity.DetatchFromScene();
            }
        }

        public void AttachToScene(Scene newScene)
        {
            Scene = newScene;
            newScene.Entities.Add(this);
            Components.RegisterAllComponents();

            for (var i = 0; i < Transform.ChildCount; i++)
            {
                Transform.GetChild(i).Entity.AttachToScene(newScene);
            }
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
            // Entity fields
            Tag = entity.Tag;
            UpdateInterval = entity.UpdateInterval;
            UpdateOrder = entity.UpdateOrder;
            Enabled = entity.Enabled;

            Transform.Scale = entity.Transform.Scale;
            Transform.Rotation = entity.Transform.Rotation;

            // clone Components
            for (var i = 0; i < entity.Components.Count; i++)
                AddComponent(entity.Components[i].Clone());
            for (var i = 0; i < entity.Components._componentsToAdd.Count; i++)
                AddComponent(entity.Components._componentsToAdd[i].Clone());

            // clone any children of the Entity.transform
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
            if (IsDestroyed) Components.RemoveAll();
        }

        public virtual void Update(GameTime gameTime)
        {
            Components.Update(gameTime);
        }

        public virtual void DegugRender(SpriteBatch spriteBatch)
        {
            Components.DebugRender(spriteBatch);
        }

        public T AddComponent<T>(T component) 
            where T : Component
        {
            component.Entity = this;
            Components.Add(component);
            component.Initialize();
            return component;
        }

        public T AddComponent<T>() 
            where T : Component, new()
        {
            var component = new T
            {
                Entity = this
            };
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
            var comp = Components.GetComponent<T>(true) ?? AddComponent<T>();

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
            if (comp == null) return false;
            RemoveComponent(comp);
            return true;
        }

        public void RemoveComponent(Component component) => Components.Remove(component);

        public void RemoveAllComponents()
        {
            foreach(var item in Components) RemoveComponent(item);
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
            return string.Format($"[Entity: Name: {Name}, Tag: {Tag}, Enabled: {Enabled}, Depth: {UpdateOrder}]");
        }
    }
}