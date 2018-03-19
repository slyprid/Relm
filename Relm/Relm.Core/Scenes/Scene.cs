using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core.Managers;

namespace Relm.Core.Scenes
{
	public abstract class Scene
		: IManaged<Scene>
	{
		public IManager<Scene> Manager { get; set; }

		public string MyAlias { get; set; }

		public bool IsEnabled { get; set; }
		public bool IsPaused { get; set; }
		public bool IsVisible { get; set; }
		public bool IsTransitioning { get; set; }
		public SpriteBatch SpriteBatch { get; set; }

        public List<SceneComponent> Components { get; set; }

		protected Scene()
		{
			IsEnabled = true;
			IsPaused = false;
			IsVisible = true;

		    Components = new List<SceneComponent>();
		}

		public static string GenerateAlias()
		{
			return Guid.NewGuid().ToString();
		}

		public virtual void StartChange(Action onSuccess)
		{
			IsTransitioning = true;

			onSuccess?.Invoke();
		}

		public virtual void EndChange(Action onSuccess)
		{
			IsTransitioning = false;

			onSuccess?.Invoke();
		}

		public virtual void Pause()
		{
			IsPaused = !IsPaused;
		}

		public virtual void Update(GameTime gameTime) { }

		public virtual void Draw(GameTime gameTime) { }
	}
}