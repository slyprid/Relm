using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Collections;
using Relm.Entities;
using Relm.Renderers.Renderables;

namespace Relm.Components
{
    public class ComponentList
        : SafeList<Component>
    {
        private readonly Entity _entity;

        public ComponentList(Entity entity)
        {
            _entity = entity;
        }

        public override void UpdateLists()
        {
            if (ItemsToRemove.Count > 0)
            {
                foreach (var item in ItemsToRemove)
                {
                    HandleRemove(item);
                    Items.Remove(item);
                }
                ItemsToRemove.Clear();
            }

            if (ItemsToAdd.Count > 0)
            {
                foreach (var item in ItemsToAdd)
                {
                    if (item is RenderableComponent) _entity.Scene.RenderableComponents.Add(item as RenderableComponent);
                    if (item is IUpdatable) ItemsToUpdate.Add(item as IUpdatable);
                    Items.Add(item);
                    TempItems.Add(item);
                }

                ItemsToAdd.Clear();

                foreach (var item in TempItems)
                {
                    item.OnAddedToEntity();
                    if (item.Enabled) item.OnEnabled();
                }

                TempItems.Clear();
            }
        }

        public override void HandleRemove(Component item)
        {
            if (item is RenderableComponent) _entity.Scene.RenderableComponents.Remove(item as RenderableComponent);

            if (item is IUpdateable) ItemsToUpdate.Remove(item as IUpdateable);

            item.OnRemovedFromEntity();
            item.Entity = null;
        }

        public T GetComponent<T>(bool onlyReturnInitializedComponents)
            where T : Component
        {
            foreach (var item in Items.OfType<T>())
            {
                return item;
            }

            return !onlyReturnInitializedComponents
                ? ItemsToAdd.OfType<T>().FirstOrDefault()
                : null;
        }

        public void GetComponents<T>(List<T> ret)
            where T : class
        {
            ret.AddRange(Items.OfType<T>().Select(item => item as T));
            ret.AddRange(ItemsToAdd.OfType<T>().Select(item => item as T));
        }

        public List<T> GetComponents<T>()
            where T : class
        {
            var ret = new List<T>();
            ret.AddRange(Items.OfType<T>().Select(item => item as T));
            ret.AddRange(ItemsToAdd.OfType<T>().Select(item => item as T));
            return ret;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateLists();
            foreach (var item in ItemsToUpdate.Where(item => item.Enabled))
            {
                item.Update(gameTime);
            }
        }

        internal void OnEntityTranformChanged(Component component)
        {
            foreach (var item in Items.Where(item => item.Enabled))
            {
                item.OnEntityTransformChanged(component);
            }

            foreach (var item in ItemsToAdd.Where(item => item.Enabled))
            {
                item.OnEntityTransformChanged(component);
            }
        }

        internal void OnEntityEnabled()
        {
            foreach (var item in Items)
            {
                item.OnEnabled();
            }
        }

        internal void OnEntityDisabled()
        {
            foreach (var item in Items)
            {
                item.OnDisabled();
            }
        }

        internal void DebugRender(SpriteBatch spriteBatch)
        {
            foreach (var item in Items)
            {
                item.DebugRender(spriteBatch);
            }
        }
    }
}