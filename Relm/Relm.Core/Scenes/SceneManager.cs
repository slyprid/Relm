using Microsoft.Xna.Framework;
using Relm.Core.Managers;

namespace Relm.Core.Scenes
{
	public class SceneManager
		: Manager<Scene>
	{
		public Scene CurrentScene { get; protected set; }

		public void ChangeState(string stateAlias)
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

		public void Add(Scene scene)
		{
			Add(Scene.GenerateAlias(), scene);
		}

		public void Update(GameTime gameTime)
		{
			if (CurrentScene == null) return;
			if (!CurrentScene.IsEnabled) return;
            CurrentScene.Update(gameTime);
		}

		public void Draw(GameTime gameTime)
		{
			if (CurrentScene == null) return;
			if (!CurrentScene.IsVisible) return;
		    CurrentScene.Draw(gameTime);
		}
	}
}