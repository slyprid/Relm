using Relm.Collections;
using Relm.Scenes;

namespace Relm.Entities
{
    public class EntityList
        : SafeList<Entity>
    {
        public Scene Scene { get; set; }

        public int Count => Items.Count;
        
        public EntityList(Scene scene)
        {
            Scene = scene;
        }

        public override void RemoveAll()
        {
            ItemsToAdd.Clear();
            UpdateLists();

            foreach (var item in Items)
            {
                item.IsDestroyed = true;
                item.OnRemovedFromScene();
            }
        }
    }
}