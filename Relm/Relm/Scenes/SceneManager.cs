using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Managers;

namespace Relm.Scenes
{
    public class SceneManager
        : Manager
    {
        private readonly Dictionary<string, Scene> _scenes = new Dictionary<string, Scene>();

        public Scene ActiveScene { get; private set; }

        public override void Update(GameTime gameTime)
        {
            ActiveScene?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            ActiveScene?.Draw(gameTime);
        }

        public Scene LoadScene(Type sceneType)
        {
            var scene = (Scene)Activator.CreateInstance(sceneType);
            _scenes.Add(scene.Name, scene);
            return scene;
        }

        public void ChangeScene(string sceneName)
        {
            var newScene = _scenes[sceneName];
            ActiveScene?.OnSceneExit();
            ActiveScene = newScene;
            ActiveScene.OnSceneChange();
            ActiveScene.OnSceneEnter();
        }
    }
}