using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Relm.Scenes
{
    public class SceneManager
    {
        public Dictionary<string, Scene> Scenes { get; set; }
        public List<string> ActiveSceneNames { get; set; }

        public Scene this[string name] => Scenes[name];

        public SceneManager()
        {
            Scenes = new Dictionary<string, Scene>();
            ActiveSceneNames = new List<string>();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var scene in ActiveSceneNames.Select(name => Scenes[name]))
            {
                scene.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            foreach (var scene in ActiveSceneNames.Select(name => Scenes[name]))
            {
                scene.Draw(gameTime);
            }
        }

        public void AddScene(string name, Scene scene)
        {
            Scenes.Add(name, scene);
        }

        public void RemoveScene(string name)
        {
            Scenes.Remove(name);
            ActiveSceneNames.Remove(name);
        }

        public void ActivateScene(string name)
        {
            ActiveSceneNames.Add(name);
            this[name].OnActivate();
        }

        public void DeactivateScene(string name)
        {
            ActiveSceneNames.Remove(name);
            this[name].OnDeactivate();
        }
    }
}