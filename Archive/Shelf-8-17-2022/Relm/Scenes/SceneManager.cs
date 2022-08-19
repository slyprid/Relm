using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Managers;

namespace Relm.Scenes
{
    public class SceneManager
        : Manager
    {
        private readonly Dictionary<string, OldScene> _scenes = new Dictionary<string, OldScene>();

        public OldScene ActiveOldScene { get; private set; }

        public override void Update(GameTime gameTime)
        {
            ActiveOldScene?.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            ActiveOldScene?.Draw(gameTime);
        }

        public OldScene LoadScene(Type sceneType)
        {
            var scene = (OldScene)Activator.CreateInstance(sceneType);
            _scenes.Add(scene.Name, scene);
            return scene;
        }

        public void ChangeScene(string sceneName)
        {
            var newScene = _scenes[sceneName];
            ActiveOldScene?.OnSceneExit();
            ActiveOldScene = newScene;
            ActiveOldScene.OnSceneChange();
            ActiveOldScene.OnSceneEnter();
        }
    }
}