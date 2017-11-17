using Microsoft.Xna.Framework;
using Relm.Core.Managers;
using Relm.Core.States;

namespace Relm.Core.Scenes
{
    public class SceneManager
        : Manager<Scene>
    {
        public Scene CurrentScene { get; protected set; }

        public void ChangeScene(string stateAlias)
        {
            if (CurrentScene == null)
            {
                CurrentScene = GetItem(stateAlias);
                return;
            }

            CurrentScene.StartChange(() =>
            {
                CurrentScene.EndChange(() =>
                {
                    CurrentScene = GetItem(stateAlias);
                });
            });
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentScene == null) return;
            if (!CurrentScene.IsEnabled) return;
        }

        public void Draw(GameTime gameTime)
        {
            if (CurrentScene == null) return;
            if (!CurrentScene.IsVisible) return;
        }
    }
}