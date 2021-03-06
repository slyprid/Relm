﻿using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Relm.Scenes
{
    public class SceneManager
    {
        private bool _isChangingScene = false;

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
            if (_isChangingScene) return;

            foreach (var scene in ActiveSceneNames.Select(name => Scenes[name]).ToList())
            {
                scene.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (_isChangingScene) return;

            foreach (var scene in ActiveSceneNames.Select(name => Scenes[name]).ToList())
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

        public Scene ActivateScene(string name)
        {
            ActiveSceneNames.Add(name);
            this[name].OnActivate();
            return this[name];
        }

        public void DeactivateScene(string name)
        {
            ActiveSceneNames.Remove(name);
            this[name].OnDeactivate();
            this[name].Cleanup();
        }

        public Scene ChangeScene(string name)
        {
            _isChangingScene = true;

            foreach (var sceneName in ActiveSceneNames.ToList())
            {
                DeactivateScene(sceneName);
            }

            var ret = ActivateScene(name);

            _isChangingScene = false;

            return ret;
        }

        public void SwapScene(string activeScene, string newScene)
        {
            DeactivateScene(activeScene);
            ActivateScene(newScene);
        }
    }
}