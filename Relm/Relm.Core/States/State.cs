using System;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core.Managers;

namespace Relm.Core.States
{
	public abstract class State
		: IManaged<State>
	{
		public IManager<State> Manager { get; set; }

		public string MyAlias { get; set; }

		public bool IsEnabled { get; set; }
		public bool IsPaused { get; set; }
		public bool IsVisible { get; set; }
		public bool IsTransitioning { get; set; }
		public SpriteBatch SpriteBatch { get; set; }

		protected State()
		{
			IsEnabled = true;
			IsPaused = false;
			IsVisible = true;
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